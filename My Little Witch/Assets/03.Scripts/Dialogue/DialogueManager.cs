using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private TMPro.TMP_Text dialogueText;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject askAWill;
    public QuestBook questBook; //세이브로드 할 때 참조
    private Animator camAnimator;
    public Queue<string> sentences;

    private DialogueTrigger curTrigger; // 전달받을 트리거 정보
    private DialogueData curData; // 전달받을 대화 데이터 값


    private void Start()
    {
        sentences = new Queue<string>();
        camAnimator = Camera.main.transform.parent.GetComponent<Animator>();
    }
    
    public void DM_StartDialogue(DialogueData _curData, DialogueTrigger _curTrigger = null)
    {
        //
        if (_curTrigger != null) curTrigger = _curTrigger;
        curData = _curData;
        //
        camAnimator.SetBool("IsInteracting", true);
        SceneData.Inst.talkSign.SetBool("IsOpen", false);
        
        animator.SetBool("IsOpen", true);
        sentences.Clear(); // 처음 읽기전에 이전 컨텐츠를 클리어
        nameText.text = curData.npcName;
        foreach (string sentence in curData.contents)
        {
            sentences.Enqueue(sentence);
        }
        DM_DisplayNextSentence();
    }

    public void DM_DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            DM_EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        //dialogueText.text = sentence;
        this.StopAllCoroutines(); // 이전 코루틴에서 실행중인 대사를 모두 중지하고 새로 시작하기 위해
        StartCoroutine(DM_TypeSentence(sentence));
    }

    public void DM_EndDialogue()
    {
        if (curData.type == DialogueData.Type.QuestGiver) //퀘스트 타입일 시 수락여부 물음
        {
            //ProgressChecker(); // 대화 타입 검사
            askAWill.SetActive(true); // 퀘스트 수락할지 물음
        }
        else
        {
            DM_ProgressChecker(); // 대화 타입 검사
            SceneData.Inst.myPlayer.OnInteraction = false; // 플레이어 다시 움직임
            animator.SetBool("IsOpen", false); // 패널 닫음
            camAnimator.SetBool("IsInteracting", false); // 카메라 원복

            // 대화종료, F키를 눌러 다시 대화를 할 수 있도록
            if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
            curTrigger.isTalking = false;
        }
    }

    public void DM_ProgressChecker()
    {
        if (curData.type == DialogueData.Type.Dialogue)
        {
            curTrigger.progress += 1;
        }
        else if (curData.type == DialogueData.Type.Reward)
        {
            DM_GetPlayerReward(); //대화 종료 후 보상 지급
            curTrigger.progress += 1;
            SceneData.Inst.questManager.QM_TurnMarkOff(curData.npcId);
            if(curData.questRewardData.type == QuestData.QuestType.Delivery)
            {
                DM_TakeOverQuestItem();
            }
        }
        else if (curData.type == DialogueData.Type.OpenStore)
        {
            SceneData.Inst.interactableUIManager.OpenStore();
        }
        else { return; }
    }

    void DM_TakeOverQuestItem()
    {
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // 슬롯 수량만큼 반복
        {
            if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item != null)
            {
                if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.myItem.orgData.name == curData.questRewardData.goalKeyword)
                //이름이 같은지 조건검사
                {
                    if(SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.curNumber == 1)
                    {
                        if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.gameObject != null) Destroy(SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.gameObject);
                        SceneData.Inst.Inven.slots[i].GetComponent<Slots>().ClearSlot(); // 1개라면 제거하고 리턴
                        return;
                    }
                    else
                    {
                        SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.curNumber--;
                        return; // 복수라면 수량 줄이고 리턴
                    }
                }
            }
        }
    }

    void DM_GetPlayerReward() // 플레이어에게 보상 전달
    {
        SceneData.Inst.myPlayer.GetEXP(curData.questRewardData.exp); // 플레이어 경험치
        SceneData.Inst.interactableUIManager.SetGold(curData.questRewardData.currency); // 플레이어 골드

        if (curData.questRewardData.reward != null)
        {
            GameObject obj = Instantiate(curData.questRewardData.reward);
            obj.GetComponent<Item>().GetItem();
        }
        

        if(questBook.content.GetChild(0).GetComponent<QuestTab>().questData.questIndex == curData.questRewardData.questIndex) //퀘스트북에서 조건검사해서 퀘스트탭 오브젝트 삭제
        {
            questBook.content.GetChild(0).GetComponent<QuestTab>().QT_DestroyQuestTab(questBook.content.GetChild(0).GetComponent<QuestTab>());
            return;// 0번 배열에 있다면 바로 지우고 반복문없이 리턴
        }
        else
        {
            for (int i = 0; i < questBook.content.childCount; i++) // 퀘스트북에서 조건검사해서 퀘스트탭 오브젝트 삭제
            {
                if (questBook.content.GetChild(i).GetComponent<QuestTab>().questData.questIndex == curData.questRewardData.questIndex)
                {
                    questBook.content.GetChild(i).GetComponent<QuestTab>().QT_DestroyQuestTab(questBook.content.GetChild(i).GetComponent<QuestTab>());
                    break;
                }
            }
        }
    }
    

    public void IfAccepted()
    {
        curTrigger.progress += 1; // 진행도를 1 올림

        if (curData.questObj != null) 
        { 
            GameObject obj = Instantiate(curData.questObj, questBook.content); // 퀘스트북에 퀘스트를 추가해줌
            SceneData.Inst.questManager.questInProgress.Add(obj.GetComponent<QuestTab>()); // 퀘스트북 '진행중' 리스트에 추가  
        }

        SceneData.Inst.interactableUIManager.OpenQuestBookAfterDialogue(); // 퀘스트창을 띄워 퀘스트 프리팹이 진행을 가능하게 해줌


        SceneData.Inst.myPlayer.OnInteraction = false; // 플레이어 다시 움직임
        animator.SetBool("IsOpen", false); // 패널 닫음
        camAnimator.SetBool("IsInteracting", false); // 카메라 원복
        // 대화종료, F키를 눌러 다시 대화를 할 수 있도록
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        curTrigger.minimapPin[0].SetActive(false); // 미니맵 핀 false, true는 <QuestManager->QM_SetNpcTrigger()> 에서

        askAWill.SetActive(false); // 수락여부팝업 닫아줌
    }

    public void IfDenied()
    {
        // 대화 진행도를 올리지 않음
        SceneData.Inst.myPlayer.OnInteraction = false; // 플레이어 다시 움직임
        animator.SetBool("IsOpen", false); // 패널 닫음
        camAnimator.SetBool("IsInteracting", false); // 카메라 원복
        // 대화종료, F키를 눌러 다시 대화를 할 수 있도록
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        askAWill.SetActive(false); // 수락여부팝업 닫아줌
    }

    IEnumerator DM_TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

}

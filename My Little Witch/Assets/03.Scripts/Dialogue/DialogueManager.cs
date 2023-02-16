using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;


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
    
    public void StartDialogue(DialogueData _curData, DialogueTrigger _curTrigger = null)
    {
        //
        if (_curTrigger != null) curTrigger = _curTrigger;
        curData = _curData;
        //
        camAnimator.SetTrigger("Interaction");
        SceneData.Inst.talkSign.SetBool("IsOpen", false);
        
        animator.SetBool("IsOpen", true);
        sentences.Clear(); // 처음 읽기전에 이전 컨텐츠를 클리어
        nameText.text = curData.npcName;
        foreach (string sentence in curData.contents)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        //dialogueText.text = sentence;
        this.StopAllCoroutines(); // 이전 코루틴에서 실행중인 대사를 모두 중지하고 새로 시작하기 위해
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        if (curData.type == DialogueData.Type.QuestGiver) //퀘스트 타입일 시 수락여부 물음
        {
            ProgressChecker(); // 대화 타입 검사
            askAWill.SetActive(true); // 퀘스트 수락할지 물음
        }
        else
        {
            ProgressChecker(); // 대화 타입 검사
            SceneData.Inst.myPlayer.OnInteraction = false; // 플레이어 다시 움직임
            animator.SetBool("IsOpen", false); // 패널 닫음
            camAnimator.SetTrigger("AsBefore"); // 카메라 원복

            // 대화종료, F키를 눌러 다시 대화를 할 수 있도록
            if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
            curTrigger.isTalking = false;
        }
    }

    public void ProgressChecker()
    {
        if (curData.type == DialogueData.Type.QuestGiver)
        { 
        }
        else if (curData.type == DialogueData.Type.Dialogue)
        {
            curTrigger.progress += 1;
        }
        else if (curData.type == DialogueData.Type.Reward)
        {
            return;
        }
        else { return; }
    }

    public void IfAccepted()
    {
        curTrigger.progress += 1; // 진행도를 1 올림
        if (curData.questObj != null) { GameObject obj = Instantiate(curData.questObj, questBook.content); } // 퀘스트북에 퀘스트를 추가해줌
        SceneData.Inst.questManager.questInProgress.Add(curData.questObj.GetComponent<QuestTab>().questData.questIndex); // 퀘스트북 '진행중' 리스트에 인덱스를 추가
        print(SceneData.Inst.questManager.questInProgress.Count);
        SceneData.Inst.myPlayer.OnInteraction = false; // 플레이어 다시 움직임
        animator.SetBool("IsOpen", false); // 패널 닫음
        camAnimator.SetTrigger("AsBefore"); // 카메라 원복
        // 대화종료, F키를 눌러 다시 대화를 할 수 있도록
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        curTrigger.minimapPin.SetActive(false); // 미니맵 핀 false

        askAWill.SetActive(false); // 수락여부팝업 닫아줌
    }

    public void IfDenied()
    {
        // 대화 진행도를 올리지 않음
        SceneData.Inst.myPlayer.OnInteraction = false; // 플레이어 다시 움직임
        animator.SetBool("IsOpen", false); // 패널 닫음
        camAnimator.SetTrigger("AsBefore"); // 카메라 원복
        // 대화종료, F키를 눌러 다시 대화를 할 수 있도록
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        curTrigger.minimapPin.SetActive(false); // 미니맵 핀 false, true는 <QuestManager->QM_SetNpcTrigger()> 에서
        askAWill.SetActive(false); // 수락여부팝업 닫아줌
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

}

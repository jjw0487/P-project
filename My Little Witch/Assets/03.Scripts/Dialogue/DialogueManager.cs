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
    public QuestBook questBook; //���̺�ε� �� �� ����
    private Animator camAnimator;
    public Queue<string> sentences;

    private DialogueTrigger curTrigger; // ���޹��� Ʈ���� ����
    private DialogueData curData; // ���޹��� ��ȭ ������ ��


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
        sentences.Clear(); // ó�� �б����� ���� �������� Ŭ����
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
        this.StopAllCoroutines(); // ���� �ڷ�ƾ���� �������� ��縦 ��� �����ϰ� ���� �����ϱ� ����
        StartCoroutine(DM_TypeSentence(sentence));
    }

    public void DM_EndDialogue()
    {
        if (curData.type == DialogueData.Type.QuestGiver) //����Ʈ Ÿ���� �� �������� ����
        {
            //ProgressChecker(); // ��ȭ Ÿ�� �˻�
            askAWill.SetActive(true); // ����Ʈ �������� ����
        }
        else
        {
            DM_ProgressChecker(); // ��ȭ Ÿ�� �˻�
            SceneData.Inst.myPlayer.OnInteraction = false; // �÷��̾� �ٽ� ������
            animator.SetBool("IsOpen", false); // �г� ����
            camAnimator.SetBool("IsInteracting", false); // ī�޶� ����

            // ��ȭ����, FŰ�� ���� �ٽ� ��ȭ�� �� �� �ֵ���
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
            DM_GetPlayerReward(); //��ȭ ���� �� ���� ����
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
        for (int i = 0; i < SceneData.Inst.Inven.slots.Length; ++i) // ���� ������ŭ �ݺ�
        {
            if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item != null)
            {
                if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.myItem.orgData.name == curData.questRewardData.goalKeyword)
                //�̸��� ������ ���ǰ˻�
                {
                    if(SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.curNumber == 1)
                    {
                        if (SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.gameObject != null) Destroy(SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.gameObject);
                        SceneData.Inst.Inven.slots[i].GetComponent<Slots>().ClearSlot(); // 1����� �����ϰ� ����
                        return;
                    }
                    else
                    {
                        SceneData.Inst.Inven.slots[i].GetComponent<Slots>().item.curNumber--;
                        return; // ������� ���� ���̰� ����
                    }
                }
            }
        }
    }

    void DM_GetPlayerReward() // �÷��̾�� ���� ����
    {
        SceneData.Inst.myPlayer.GetEXP(curData.questRewardData.exp); // �÷��̾� ����ġ
        SceneData.Inst.interactableUIManager.SetGold(curData.questRewardData.currency); // �÷��̾� ���

        if (curData.questRewardData.reward != null)
        {
            GameObject obj = Instantiate(curData.questRewardData.reward);
            obj.GetComponent<Item>().GetItem();
        }
        

        if(questBook.content.GetChild(0).GetComponent<QuestTab>().questData.questIndex == curData.questRewardData.questIndex) //����Ʈ�Ͽ��� ���ǰ˻��ؼ� ����Ʈ�� ������Ʈ ����
        {
            questBook.content.GetChild(0).GetComponent<QuestTab>().QT_DestroyQuestTab(questBook.content.GetChild(0).GetComponent<QuestTab>());
            return;// 0�� �迭�� �ִٸ� �ٷ� ����� �ݺ������� ����
        }
        else
        {
            for (int i = 0; i < questBook.content.childCount; i++) // ����Ʈ�Ͽ��� ���ǰ˻��ؼ� ����Ʈ�� ������Ʈ ����
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
        curTrigger.progress += 1; // ���൵�� 1 �ø�

        if (curData.questObj != null) 
        { 
            GameObject obj = Instantiate(curData.questObj, questBook.content); // ����Ʈ�Ͽ� ����Ʈ�� �߰�����
            SceneData.Inst.questManager.questInProgress.Add(obj.GetComponent<QuestTab>()); // ����Ʈ�� '������' ����Ʈ�� �߰�  
        }

        SceneData.Inst.interactableUIManager.OpenQuestBookAfterDialogue(); // ����Ʈâ�� ��� ����Ʈ �������� ������ �����ϰ� ����


        SceneData.Inst.myPlayer.OnInteraction = false; // �÷��̾� �ٽ� ������
        animator.SetBool("IsOpen", false); // �г� ����
        camAnimator.SetBool("IsInteracting", false); // ī�޶� ����
        // ��ȭ����, FŰ�� ���� �ٽ� ��ȭ�� �� �� �ֵ���
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        curTrigger.minimapPin[0].SetActive(false); // �̴ϸ� �� false, true�� <QuestManager->QM_SetNpcTrigger()> ����

        askAWill.SetActive(false); // ���������˾� �ݾ���
    }

    public void IfDenied()
    {
        // ��ȭ ���൵�� �ø��� ����
        SceneData.Inst.myPlayer.OnInteraction = false; // �÷��̾� �ٽ� ������
        animator.SetBool("IsOpen", false); // �г� ����
        camAnimator.SetBool("IsInteracting", false); // ī�޶� ����
        // ��ȭ����, FŰ�� ���� �ٽ� ��ȭ�� �� �� �ֵ���
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        askAWill.SetActive(false); // ���������˾� �ݾ���
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

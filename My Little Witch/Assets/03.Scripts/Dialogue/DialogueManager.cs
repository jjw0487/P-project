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
    
    public void StartDialogue(DialogueData _curData, DialogueTrigger _curTrigger = null)
    {
        //
        if (_curTrigger != null) curTrigger = _curTrigger;
        curData = _curData;
        //
        camAnimator.SetTrigger("Interaction");
        SceneData.Inst.talkSign.SetBool("IsOpen", false);
        
        animator.SetBool("IsOpen", true);
        sentences.Clear(); // ó�� �б����� ���� �������� Ŭ����
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
        this.StopAllCoroutines(); // ���� �ڷ�ƾ���� �������� ��縦 ��� �����ϰ� ���� �����ϱ� ����
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        if (curData.type == DialogueData.Type.QuestGiver) //����Ʈ Ÿ���� �� �������� ����
        {
            ProgressChecker(); // ��ȭ Ÿ�� �˻�
            askAWill.SetActive(true); // ����Ʈ �������� ����
        }
        else
        {
            ProgressChecker(); // ��ȭ Ÿ�� �˻�
            SceneData.Inst.myPlayer.OnInteraction = false; // �÷��̾� �ٽ� ������
            animator.SetBool("IsOpen", false); // �г� ����
            camAnimator.SetTrigger("AsBefore"); // ī�޶� ����

            // ��ȭ����, FŰ�� ���� �ٽ� ��ȭ�� �� �� �ֵ���
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
        curTrigger.progress += 1; // ���൵�� 1 �ø�
        if (curData.questObj != null) { GameObject obj = Instantiate(curData.questObj, questBook.content); } // ����Ʈ�Ͽ� ����Ʈ�� �߰�����
        SceneData.Inst.questManager.questInProgress.Add(curData.questObj.GetComponent<QuestTab>().questData.questIndex); // ����Ʈ�� '������' ����Ʈ�� �ε����� �߰�
        print(SceneData.Inst.questManager.questInProgress.Count);
        SceneData.Inst.myPlayer.OnInteraction = false; // �÷��̾� �ٽ� ������
        animator.SetBool("IsOpen", false); // �г� ����
        camAnimator.SetTrigger("AsBefore"); // ī�޶� ����
        // ��ȭ����, FŰ�� ���� �ٽ� ��ȭ�� �� �� �ֵ���
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        curTrigger.minimapPin.SetActive(false); // �̴ϸ� �� false

        askAWill.SetActive(false); // ���������˾� �ݾ���
    }

    public void IfDenied()
    {
        // ��ȭ ���൵�� �ø��� ����
        SceneData.Inst.myPlayer.OnInteraction = false; // �÷��̾� �ٽ� ������
        animator.SetBool("IsOpen", false); // �г� ����
        camAnimator.SetTrigger("AsBefore"); // ī�޶� ����
        // ��ȭ����, FŰ�� ���� �ٽ� ��ȭ�� �� �� �ֵ���
        if (!SceneData.Inst.talkSign.GetBool("IsOpen")) SceneData.Inst.talkSign.SetBool("IsOpen", true);
        curTrigger.isTalking = false;
        curTrigger.minimapPin.SetActive(false); // �̴ϸ� �� false, true�� <QuestManager->QM_SetNpcTrigger()> ����
        askAWill.SetActive(false); // ���������˾� �ݾ���
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

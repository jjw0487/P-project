using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private TMPro.TMP_Text dialogueText;
    [SerializeField] private Animator animator;
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
        camAnimator.SetTrigger("Interaction");
        SceneData.Inst.talkSign.SetBool("IsOpen", false);
        //
        if(_curTrigger != null) curTrigger = _curTrigger;
        curData = _curData;
        //
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
        ProgressChecker();
        SceneData.Inst.myPlayer.OnInteraction = false;
        animator.SetBool("IsOpen", false);
        camAnimator.SetTrigger("AsBefore");
        curTrigger.isTalking = false; // ��ȭ����, FŰ�� ���� �ٽ� ��ȭ�� �� �� �ֵ���
    }

    public void ProgressChecker()
    {
        if (curData.type == DialogueData.Type.QuestGiver)
        {
            curTrigger.GetComponent<DialogueTrigger>().progress += 1; // ��ȭ ���� �� ���൵�� 1 �ø�
        }
        else if (curData.type == DialogueData.Type.Repeat)
        {
            return;
        }
        else if (curData.type == DialogueData.Type.Dialogue)
        {
            curTrigger.GetComponent<DialogueTrigger>().progress += 1;
        }
        else if (curData.type == DialogueData.Type.Reward)
        {
            return;
        }
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

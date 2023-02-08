using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private TMPro.TMP_Text dialogueText;
    [SerializeField] private Animator animator;
    public Queue<string> sentences;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData dialogue)
    {
        animator.SetBool("IsOpen", true);

        sentences.Clear(); // ó�� �б����� ���� �������� Ŭ����

        nameText.text = dialogue.npcName;

        foreach (string sentence in dialogue.contents)
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
        StopAllCoroutines(); // ���� �ڷ�ƾ���� �������� ��縦 ��� �����ϰ� ���� �����ϱ� ����
        StartCoroutine(TypeSentence(sentence));

    }

    public void EndDialogue()
    {
        // interaction => false �����ְ� ��Ÿ Ʈ���� �۵�
        animator.SetBool("IsOpen", false);
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

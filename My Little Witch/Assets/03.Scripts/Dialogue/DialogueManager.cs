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

        sentences.Clear(); // 처음 읽기전에 이전 컨텐츠를 클리어

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
        StopAllCoroutines(); // 이전 코루틴에서 실행중인 대사를 모두 중지하고 새로 시작하기 위해
        StartCoroutine(TypeSentence(sentence));

    }

    public void EndDialogue()
    {
        // interaction => false 시켜주고 기타 트리거 작동
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

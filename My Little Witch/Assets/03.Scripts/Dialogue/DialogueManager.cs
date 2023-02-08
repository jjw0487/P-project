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
    public Queue<string> sentences;

    private DialogueData curDialogue;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData dialogue)
    {
        curDialogue = dialogue;
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
        this.StopAllCoroutines(); // 이전 코루틴에서 실행중인 대사를 모두 중지하고 새로 시작하기 위해
        StartCoroutine(TypeSentence(sentence));

    }

    public void EndDialogue()
    {
        curDialogue.GetComponent<DialogueTrigger>().progress += 1; // 대화 종료 후 진행도를 1 올림
        Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("AsBefore");
        SceneData.Inst.myPlayer.OnInteraction = false;
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

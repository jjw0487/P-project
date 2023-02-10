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

    private DialogueTrigger curTrigger; // 전달받을 트리거 정보
    private DialogueData curData; // 전달받을 대화 데이터 값

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
        ProgressChecker();
        SceneData.Inst.myPlayer.OnInteraction = false;
        animator.SetBool("IsOpen", false);
        camAnimator.SetTrigger("AsBefore");
        curTrigger.isTalking = false; // 대화종료, F키를 눌러 다시 대화를 할 수 있도록
    }

    public void ProgressChecker()
    {
        if (curData.type == DialogueData.Type.QuestGiver)
        {
            curTrigger.GetComponent<DialogueTrigger>().progress += 1; // 대화 종료 후 진행도를 1 올림
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

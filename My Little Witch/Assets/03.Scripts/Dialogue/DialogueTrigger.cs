using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData[] dialogue;
    public int progress;

    public void StartConversation()
    {
        if (dialogue[progress].type == DialogueData.Type.QuestGiver)
        {
            // 퀘스트가 
            // 퀘스트를 체크한 후 완료했다면 리워드 대화로 진행
        }
        else if (dialogue[progress].type == DialogueData.Type.Reward)
        {

        }
        else if(dialogue[progress].type == DialogueData.Type.Dialogue)
        {
            SceneData.Inst.dialogueManager.StartDialogue(this.dialogue[progress], this);
        }
    }

}

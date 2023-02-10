using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData[] dialogue;
    public GameObject minimapPin;
    public int progress;
    public bool isTalking;

    public void StartConversation()
    {
        if (dialogue[progress].type == DialogueData.Type.QuestGiver)
        {
            SceneData.Inst.dialogueManager.StartDialogue(this.dialogue[progress], this);
            // ����Ʈ�� üũ�� �� �Ϸ��ߴٸ� ������ ��ȭ�� ����
        }
        else if (dialogue[progress].type == DialogueData.Type.Repeat)
        {
            SceneData.Inst.dialogueManager.StartDialogue(this.dialogue[progress], this);
        }
        else if(dialogue[progress].type == DialogueData.Type.Dialogue)
        {
            SceneData.Inst.dialogueManager.StartDialogue(this.dialogue[progress], this);
        }
        else if (dialogue[progress].type == DialogueData.Type.Reward)
        {
            SceneData.Inst.dialogueManager.StartDialogue(this.dialogue[progress], this);
        }
    }

}

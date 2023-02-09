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
            // ����Ʈ�� 
            // ����Ʈ�� üũ�� �� �Ϸ��ߴٸ� ������ ��ȭ�� ����
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

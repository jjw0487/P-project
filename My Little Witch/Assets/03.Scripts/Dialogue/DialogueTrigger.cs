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
        SceneData.Inst.dialogueManager.StartDialogue(this.dialogue[progress]);
    }

}

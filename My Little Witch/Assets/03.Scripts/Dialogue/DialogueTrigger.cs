using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;

    public void StartConversation()
    {
        SceneData.Inst.dialogueManager.StartDialogue(this.dialogue);
    }

}

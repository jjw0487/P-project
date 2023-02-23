using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData[] dialogue;
    public GameObject[] minimapPin;
    public Transform talkMarkSignPos;
    public int progress;
    public bool isTalking;

    public void StartConversation()
    {
        SceneData.Inst.dialogueManager.DM_StartDialogue(this.dialogue[progress], this);
    }

}

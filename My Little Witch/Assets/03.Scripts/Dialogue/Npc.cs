using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected DialogueTrigger dialogue;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            SceneData.Inst.talkSign.SetBool("IsOpen", true);
            //SceneData.Inst.myPlayer.GetInteraction(this.transform);
            
            // 아 y축으로만 움직이게 바꾸자

            //transform.LookAt(other.transform);
            transform.rotation = Quaternion.LookRotation((other.transform.position - this.transform.position).normalized);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            //DialogueSign.NextAction(); sign을 대화시작하면 지워주자
            transform.LookAt(other.transform);
            dialogue.StartConversation();
            SceneData.Inst.myPlayer.GetInteraction(this.transform);

        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            SceneData.Inst.talkSign.SetBool("IsOpen", false);
        }
    }
}

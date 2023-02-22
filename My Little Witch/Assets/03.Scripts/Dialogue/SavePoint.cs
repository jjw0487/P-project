using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : Npc
{
    protected override void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            SceneData.Inst.talkSign.SetBool("IsOpen", true);
        }
    }

    protected override void OnTriggerStay(Collider other)
    {

        if (Input.GetKeyDown(KeyCode.F) && !isTalking)
        {
            isTalking = true;
            StartConversation();
            SceneData.Inst.myPlayer.GetInteraction(this.transform);
        }
    }
}

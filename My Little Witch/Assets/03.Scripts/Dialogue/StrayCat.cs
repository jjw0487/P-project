using UnityEngine;

public class StrayCat : Npc
{
    protected override void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            SceneData.Inst.talkSign.SetBool("IsOpen", true);

            //Quaternion dir = Quaternion.LookRotation((other.transform.position - this.transform.position).normalized);
            //transform.rotation = Quaternion.Euler(0f, dir.eulerAngles.y, 0f);
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

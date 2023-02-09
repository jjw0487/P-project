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
            
            // �� y�����θ� �����̰� �ٲ���

            //transform.LookAt(other.transform);
            transform.rotation = Quaternion.LookRotation((other.transform.position - this.transform.position).normalized);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            //DialogueSign.NextAction(); sign�� ��ȭ�����ϸ� ��������
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : DialogueTrigger
{

    [SerializeField] protected LayerMask layerMask;
    
    private void Start()
    {
        isTalking = false;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            SceneData.Inst.talkSign.SetBool("IsOpen", true);
            //SceneData.Inst.myPlayer.GetInteraction(this.transform);

            // y�����θ� �����̰� �ٲ���
            //transform.LookAt(other.transform);
            Quaternion dir = Quaternion.LookRotation((other.transform.position - this.transform.position).normalized);
            transform.rotation = Quaternion.Euler(0f, dir.eulerAngles.y, 0f);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F) && !isTalking)
        {
            isTalking = true;
            //DialogueSign.NextAction(); sign�� ��ȭ�����ϸ� ��������
            Quaternion dir = Quaternion.LookRotation((other.transform.position - this.transform.position).normalized);
            transform.rotation = Quaternion.Euler(0f, dir.eulerAngles.y, 0f);
            StartConversation();
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

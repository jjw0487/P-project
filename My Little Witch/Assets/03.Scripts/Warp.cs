using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected GameObject askifWarp;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            SceneData.Inst.talkSign.SetBool("IsOpen", true); // press f key
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneData.Inst.myPlayer.GetInteraction(this.transform);
            askifWarp.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
        }
    }

    
}

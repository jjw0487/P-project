using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private DialogueTrigger dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {
            //SceneData.Inst.myPlayer.GetInteraction(this.transform);
            transform.LookAt(other.transform);
        }
    }
}

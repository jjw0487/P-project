using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChecker : MonoBehaviour
{
    public LayerMask enemyMask;
    public Obstacles myStones;
    BoxCollider myBox;

    private void OnTriggerEnter(Collider other)
    {
        
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myStones.DropStones();
            print("³«ÇÏ");
        }

        myBox = this.GetComponent<BoxCollider>();
        myBox.enabled = false;
    }

}

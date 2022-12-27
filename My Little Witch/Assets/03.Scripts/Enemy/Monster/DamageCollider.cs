using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class DamageCollider : MonoBehaviour
{
    Movement myTarget = null;
    public LayerMask enemyMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myTarget = other.transform.parent.GetComponent<Movement>();
        }
        Destroy(gameObject);
        CheckCollision();
    }

    IEnumerator CheckCollision()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

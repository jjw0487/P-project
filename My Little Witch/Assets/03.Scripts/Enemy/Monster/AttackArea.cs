using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class AttackArea : MonoBehaviour
{
    Monster monster;
    public bool DontTarget = false;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!monster.isDead)
        {
            if ((monster.enemyMask & 1 << other.gameObject.layer) != 0)
            {
                monster.ChangeState(MonsterState.Attack);
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        DontTarget = true;
    }

    private void OnTriggerExit(Collider other)
    {
        DontTarget = false;
        if (!monster.isDead)
        {
            StartCoroutine(ChasingAfterAttack(1f));
        }
    }

    IEnumerator ChasingAfterAttack(float chill)
    {   
        yield return new WaitForSeconds(chill);
        monster.ChangeState(MonsterState.Target);
    }

}

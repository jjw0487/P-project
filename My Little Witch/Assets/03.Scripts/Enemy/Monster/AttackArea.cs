using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class AttackArea : MonoBehaviour
{
    Monster monster;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((monster.enemyMask & 1 << other.gameObject.layer) != 0)
        {
            monster.ChangeState(MonsterState.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(ChasingAfterAttack(1f));
    }

    IEnumerator ChasingAfterAttack(float chill)
    {   
        yield return new WaitForSeconds(chill);
        monster.ChangeState(MonsterState.Target);
    }

}

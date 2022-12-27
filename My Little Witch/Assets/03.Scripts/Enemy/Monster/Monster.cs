using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;


[Serializable]
public struct MonsterStat
{
    public MonData orgData;
    public float curHP;
}

public class Monster : CharacterProperty
{
    [Header("Monster Data")]
    public MonsterStat monStat;
    public Transform myAttakPoint;
    public enum MonsterState
    {
        Create, Idle, Roam, Target, Attack, Dead
    }
    public MonsterState state = MonsterState.Create;
    protected Vector3 roamPos;
    protected Vector3 IdlePos;
    protected AttackArea myAttackArea;
    

    [Header("Target")]
    public Transform target;
    public LayerMask enemyMask;
    protected Movement myEnemy;
    protected Vector3 targetDir;

    public void ChangeState(MonsterState what)
    {
        if (state == what) return;
        state = what;
        switch(state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                monStat.curHP = monStat.orgData.HP;
                myAgent.speed = monStat.orgData.agentSpeed;
                StartCoroutine(DelayRoaming(3f));
                myAgent.SetDestination(IdlePos);
                break;
            case MonsterState.Roam:
                Vector3 rndPos = Vector3.zero;
                rndPos.x = UnityEngine.Random.Range(-1.5f, 1.5f);
                rndPos.z = UnityEngine.Random.Range(-1.5f, 1.5f);
                roamPos = transform.position + rndPos;
                myAgent.SetDestination(roamPos);
                StartCoroutine(DelayIdle(3f));
                break;
            case MonsterState.Target:
                StopAllCoroutines();
                break;
            case MonsterState.Attack:
                StopAllCoroutines();
                myAttackArea.StopAllCoroutines();
                myAgent.SetDestination(transform.position);
                this.transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized);
                myAnim.SetTrigger("Attack");
                StartCoroutine(Attacking(3f));
                break;
            case MonsterState.Dead:
                StopAllCoroutines();
                myAnim.SetTrigger("Death");
                break;
        }
    }

    public void StateProcess()
    {
        switch (state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                if (myAgent.remainingDistance > 0.1f)
                {
                    myAnim.SetBool("IsRunning", true);
                }
                else
                {
                    myAnim.SetBool("IsRunning", false);
                }
                break;
            case MonsterState.Roam:
                if (myAgent.remainingDistance > 0.1f)
                {
                    myAnim.SetBool("IsRunning", true);
                }
                else
                {
                    myAnim.SetBool("IsRunning", false);
                }
                break;
            case MonsterState.Target:
                if (myEnemy != null)
                {
                    targetDir = target.transform.position;
                    myAgent.SetDestination(targetDir);
                }
                if (myAgent.remainingDistance > 0.1f)
                {
                    myAnim.SetBool("IsRunning", true);
                }
                else
                {
                    myAnim.SetBool("IsRunning", false);
                }
                break;
            case MonsterState.Attack:
                if (myAgent.remainingDistance > 0.1f)
                {
                    myAnim.SetBool("IsRunning", true);
                }
                else
                {
                    myAnim.SetBool("IsRunning", false);
                }
                break;
            case MonsterState.Dead:
                break;
        }
    }
    private void Awake()
    {
        myAttackArea = GetComponentInChildren<AttackArea>(); 
    }
    private void Start()
    {
        ChangeState(MonsterState.Idle);
        IdlePos = this.transform.position;
    }

    private void Update()
    {
        StateProcess();
    }

    private void OnTriggerEnter(Collider other)
    {
        //behaviorstate -> exit ÇÒ ¶§ 
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myEnemy = other.transform.parent.GetComponent<Movement>();
            ChangeState(MonsterState.Target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myEnemy = null;
        ChangeState(MonsterState.Idle);
    }

    public void MonAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(myAttakPoint.position, monStat.orgData.attackRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.name == "KIKI")
            {
                myEnemy.OnDmg(monStat.orgData.AT);
                break;
            }
            if (col.name == "Broom")
            {
                myEnemy.OnDmg(monStat.orgData.AT);
                break;
            }
        }
    }

    public void OnDamage(float dmg)
    {
        monStat.curHP -= dmg;
        if (monStat.curHP <= Mathf.Epsilon)
        {
           // OnDead();
        }
    }

    IEnumerator Attacking(float chill)
    {
        yield return new WaitForSeconds(chill);
        myAnim.SetTrigger("Attack");
        StartCoroutine(Attacking(chill));

    }

    IEnumerator DelayRoaming(float chill)
    {
        yield return new WaitForSeconds(chill);
        ChangeState(MonsterState.Roam);
    }

    IEnumerator DelayIdle(float chill)
    {
        yield return new WaitForSeconds(chill);
        ChangeState(MonsterState.Idle);
    }




}

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
    public bool isDead;


    [Header("Target")]
    public Transform myTarget = null;
    public LayerMask enemyMask;
    protected Movement myEnemy;
    
    protected Vector3 targetPos;
    protected Vector3 targetDir;
    protected float targetDist;

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
                myAgent.stoppingDistance = monStat.orgData.agentStopDist;
                myAgent.SetDestination(IdlePos);
                StartCoroutine(DelayRoaming(3f));
                break;
            case MonsterState.Roam:
                Vector3 rndPos = Vector3.zero;
                rndPos.x = UnityEngine.Random.Range(-2.5f, 2.5f);
                rndPos.z = UnityEngine.Random.Range(-2.5f, 2.5f);
                roamPos = transform.position + rndPos;
                myAgent.SetDestination(roamPos);
                StartCoroutine(DelayIdle(3f));
                break;
            case MonsterState.Target:
                StopAllCoroutines();
                print("T");
                break;
            case MonsterState.Attack:
                myAgent.SetDestination(transform.position);
                print("A");
                StartCoroutine(Attacking(monStat.orgData.attackSpeed));
                StartCoroutine(EnemyCheck());
                break;
            case MonsterState.Dead:
                StopAllCoroutines();
                StartCoroutine(DelayDead(4f));
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
                    targetPos = myTarget.transform.position;
                    myAgent.SetDestination(targetPos);
                }

                if(myAgent.remainingDistance <= monStat.orgData.strikingDist)
                {
                    ChangeState(MonsterState.Attack);
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
        isDead = false;
    }

    private void Update()
    {
        if(!isDead)
        { 
            StateProcess();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Enter");
        //behaviorstate -> exit ÇÒ ¶§ 
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myTarget = other.transform.parent.GetComponent<Transform>();
            myEnemy = other.transform.parent.GetComponent<Movement>();
            ChangeState(MonsterState.Target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("Exit");
        myTarget = null;
        myEnemy = null;
        if(!isDead)
        {
            ChangeState(MonsterState.Idle);
        }
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
        monStat.curHP -= dmg - monStat.orgData.DP;
        myAgent.SetDestination(transform.position);
        myAnim.SetTrigger("IsHit");
        if (monStat.curHP <= Mathf.Epsilon)
        {
            ChangeState(MonsterState.Dead);
        }
    }

    public void OnDebuff(float time, float percents)
    {
        StartCoroutine(Debuff(time, percents));
    }

    public void OnDead()
    {
        
    }

    IEnumerator Attacking(float chill)
    {
        print("Co_Attack");
        myAgent.SetDestination(transform.position);
        this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);
        myAnim.SetTrigger("Attack");

        yield return new WaitForSeconds(chill);

        StartCoroutine(Attacking(chill));
    }

    IEnumerator EnemyCheck()
    {
        while(myEnemy != null)
        {
            print("EnemyCheck");
            targetDir = this.transform.position - myTarget.transform.position;
            targetDist = targetDir.magnitude;
            if (targetDist > monStat.orgData.strikingDist + 0.2f)
            {
                ChangeState(MonsterState.Target);
                yield break;
            }
            yield return null;
        }
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
    IEnumerator DelayDead(float chill)
    {
        myAgent.SetDestination(transform.position);
        myAnim.SetTrigger("Death");
        isDead = true;
        while (chill > 0.0f)
        {
            chill -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator Debuff(float chill, float percents)
    {
        yield return null;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    public bool isDead;
    protected Vector3 roamPos;
    protected Vector3 IdlePos;
    protected AttackArea myAttackArea;


    [Header("Target")]
    public Transform myTarget = null;
    public LayerMask enemyMask;
    protected Movement myEnemy;
    
    protected Vector3 targetPos;
    protected Vector3 targetDir;
    protected float targetDist;

    Coroutine c = null;
    Coroutine idle = null;
    Coroutine attack = null;

    private bool isAttacking;

    public void ChangeState(MonsterState what)
    {
        if (state == what) return;
        state = what;
        switch(state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                isAttacking = false;
                myAgent.SetDestination(IdlePos);
                break;
            case MonsterState.Roam:
                isAttacking = false;
                Vector3 rndPos = Vector3.zero;
                rndPos.x = UnityEngine.Random.Range(-2.5f, 2.5f);
                rndPos.z = UnityEngine.Random.Range(-2.5f, 2.5f);
                roamPos = transform.position + rndPos;
                myAgent.SetDestination(roamPos);
                idle = StartCoroutine(DelayIdle(3f));
                break;
            case MonsterState.Target:
                if(idle != null) { StopCoroutine(idle);}
                if(attack != null) 
                {
                    StopCoroutine(attack);
                }
                isAttacking = false;
                break;
            case MonsterState.Attack:
                print($"{this.name} : 어택 들어옴");
                myAgent.SetDestination(transform.position);

                /*if (attack != null)
                {
                    StopCoroutine(attack);
                }*/
                attack = StartCoroutine(Attacking(monStat.orgData.attackSpeed)); //코루틴을 시작하며, 동시에 저장한다.         
                StartCoroutine(EnemyCheck());
                break;
            case MonsterState.Dead:
                isAttacking = false;
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

                targetDir = myTarget.transform.position - this.transform.position;
                targetDist = targetDir.magnitude;
                targetPos = myTarget.transform.position;

                if (myEnemy != null)
                {
                    myAgent.SetDestination(targetPos);
                }

                if(targetDist <= monStat.orgData.strikingDist)
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
        monStat.curHP = monStat.orgData.HP;
        myAgent.speed = monStat.orgData.agentSpeed;
        myAgent.stoppingDistance = monStat.orgData.agentStopDist;
        //GetComponentInChildren<Renderer>().material.enableInstancing = false;
    }

    private void Update()
    {
        if(!isDead)
        { 
            StateProcess();
        }
    }
    public void OnMouseHover()
    {
        GetComponentInChildren<Renderer>().material.SetFloat("_UseEmission", 1.0f);
    }
    public void OnMouseHoverExit()
    {
        GetComponentInChildren<Renderer>().material.SetFloat("_UseEmission", 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //behaviorstate -> exit 할 때 
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myTarget = other.transform.parent.GetComponent<Transform>();
            myEnemy = other.transform.parent.GetComponent<Movement>();
            ChangeState(MonsterState.Target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myTarget = null;
            myEnemy = null;
            if (!isDead)
            {
                ChangeState(MonsterState.Idle);
            }
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
        if (c != null) 
        {
            StopCoroutine(c);
            c = null;
        }
        c = StartCoroutine(Debuff(time, percents));
        // 강사님께 여쭤보자 !
    }

    IEnumerator Attacking(float chill)
    {
        isAttacking = true;
        print($"{this.name} : Start_C");
        myAgent.SetDestination(transform.position);
        this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);

        float cool = 0.3f;
        while(cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }

        myAnim.SetTrigger("Attack");

        while(chill > 0.0f)
        {
            chill -= Time.deltaTime;
            yield return null;
        }
        attack = StartCoroutine(Attacking(monStat.orgData.attackSpeed));
        print("실행");
    }

    IEnumerator EnemyCheck()
    {
        
        while (myEnemy != null)
        {
            targetDir = myTarget.transform.position - this.transform.position;
            targetDist = targetDir.magnitude;
            if (targetDist > monStat.orgData.strikingDist)
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
        Color color = Color.cyan;
        this.GetComponentInChildren<Renderer>().material.color = color;
        myAgent.speed = monStat.orgData.agentSpeed * percents;

        while (chill > 0.0f)
        {
            chill -= Time.deltaTime;
            yield return null;
        }

        myAgent.speed = monStat.orgData.agentSpeed;
        this.GetComponentInChildren<Renderer>().material.color = Color.white;
    }

}

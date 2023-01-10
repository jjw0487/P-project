using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    Coroutine roam = null;
    Coroutine attack = null;

    [Header("UI")]
    GameObject obj;
    Slider HPSlider;
    public Transform myHpPos;
    public Transform repositHPBars;

    public void ChangeState(MonsterState what)
    {
        print($"{this.name} : {what}");
        if (state == what) return;
        state = what;
        switch(state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                myAgent.SetDestination(IdlePos);
                roam = StartCoroutine(DelayRoaming(5f));
                break;
            case MonsterState.Roam:
                Vector3 rndPos = Vector3.zero;
                rndPos.x = UnityEngine.Random.Range(-2.5f, 2.5f);
                rndPos.z = UnityEngine.Random.Range(-2.5f, 2.5f);
                roamPos = transform.position + rndPos;
                myAgent.SetDestination(roamPos);
                idle = StartCoroutine(DelayIdle(5f));
                break;
            case MonsterState.Target:
                if(idle != null) { StopCoroutine(idle); idle = null; }
                if(roam != null) { StopCoroutine(roam); roam = null; }
                if(attack != null) { StopCoroutine(attack); attack = null; }
                break;
            case MonsterState.Attack:
                myAgent.SetDestination(transform.position);
                attack = StartCoroutine(Attacking(monStat.orgData.attackSpeed)); //코루틴을 시작하며, 동시에 저장한다.         
                StartCoroutine(EnemyCheck());
                break;
            case MonsterState.Dead:
                StopAllCoroutines();
                Destroy(obj);
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
        obj = Instantiate(Resources.Load("Monster/MonHP"), repositHPBars) as GameObject;
        obj.GetComponent<MonsterHP>().myTarget = myHpPos;
        obj.transform.localScale = monStat.orgData.HPlocalScale;
        HPSlider = obj.GetComponent<MonsterHP>().myBar;
        ChangeState(MonsterState.Idle);
        IdlePos = this.transform.position;
        isDead = false;
        monStat.curHP = monStat.orgData.HP;
        myAgent.speed = monStat.orgData.agentSpeed;
        myAgent.stoppingDistance = monStat.orgData.agentStopDist;
    }

    private void Update()
    {
        
        if (!isDead)
        {
            HPSlider.value = Mathf.Lerp(HPSlider.value, monStat.curHP / monStat.orgData.HP * 100.0f, 10.0f * Time.deltaTime);
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
        myAgent.SetDestination(transform.position);
        monStat.curHP -= dmg - monStat.orgData.DP;
        if (monStat.curHP < 0.0f)
        {
            ChangeState(MonsterState.Dead);
        }
        else
        {
            myAnim.SetTrigger("IsHit");
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
    }

    IEnumerator Attacking(float chill)
    {
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

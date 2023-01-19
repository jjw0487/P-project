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
    public Movement myEnemy = null;

    protected Vector3 targetPos;
    protected Vector3 targetDir;
    protected float targetDist;

    private bool onBattle;

    Coroutine c = null;
    Coroutine idle = null;
    Coroutine roam = null;
    Coroutine attack = null;

    [Header("UI")]
    GameObject hpObj;
    GameObject floatingDmg;
    Slider HPSlider;

    public Transform myHpPos;
    public Transform myDmgPos;

    public void ChangeState(MonsterState what)
    {
        if (state == what) return;
        state = what;
        switch (state)
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
                if (idle != null) { StopCoroutine(idle); idle = null; }
                if (roam != null) { StopCoroutine(roam); roam = null; }
                if (attack != null) { StopCoroutine(attack); attack = null; }
                break;
            case MonsterState.Attack:
                myAgent.SetDestination(transform.position);
                attack = StartCoroutine(Attacking(monStat.orgData.attackSpeed)); //코루틴을 시작하며, 동시에 저장한다.         
                //StartCoroutine(EnemyCheck());
                break;
            case MonsterState.Dead:
                StopAllCoroutines();
                Destroy(hpObj);
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

                if (myTarget != null)
                {
                    myAgent.SetDestination(targetPos);
                }

                if (targetDist <= monStat.orgData.strikingDist)
                {
                    ChangeState(MonsterState.Attack);
                }

                if (myAgent.remainingDistance > 0.2f)
                {
                    myAnim.SetBool("IsRunning", true);
                }
                else
                {
                    myAnim.SetBool("IsRunning", false);
                }

                if (Vector3.Distance(myTarget.transform.position, this.transform.position) < 7.0f)
                {
                    OnExitMotion();
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
    }

    private void Update()
    {
        if (!isDead)
        {
            if (onBattle)
            {
                HPSlider.value = Mathf.Lerp(HPSlider.value, monStat.curHP / monStat.orgData.HP * 100.0f, 10.0f * Time.deltaTime);
            }

            if (myAnim.GetBool("IsAttacking") == false)
            {
                StateProcess();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //behaviorstate -> exit 할 때 
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            //
            onBattle = true;
            hpObj = Instantiate(Resources.Load("Monster/MonHP"), SceneData.Inst.HPBars) as GameObject;
            hpObj.GetComponent<MonsterHP>().myTarget = myHpPos;
            hpObj.transform.localScale = monStat.orgData.HPlocalScale;
            HPSlider = hpObj.GetComponent<MonsterHP>().myBar;
            //
            if (myTarget == null) { myTarget = other.transform.parent.GetComponent<Transform>(); }
            if (myEnemy == null) { myEnemy = other.transform.parent.GetComponent<Movement>(); }
            ChangeState(MonsterState.Target);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myTarget = null;
            myEnemy = null;
            //
            onBattle = false;
            Destroy(hpObj);
            //
            if (!isDead)
            {
                ChangeState(MonsterState.Idle);
            }
        }
    }

    public void OnExitMotion()
    {
       
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
            if (col.name == "Guoba2")
            {
                myTarget.GetComponent<Guoba_M>().OnDmg(monStat.orgData.AT);
                break;
            }
        }
    }

    public void OnDamage(float dmg)
    {
        myAgent.SetDestination(transform.position);
        float damage = dmg - monStat.orgData.DP;
        if (damage < 0) { damage = 0; }
        monStat.curHP -= damage;
        floatingDmg = Instantiate(Resources.Load("UI/Dmg"), SceneData.Inst.FloatingDmg) as GameObject;
        floatingDmg.GetComponent<FloatingDamage>().myPos = myDmgPos;
        floatingDmg.GetComponent<FloatingDamage>().dmg.text = damage.ToString();

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

        float cool = 0.3f; // 공격을 시작 전 0.3 만큼의 대기
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }

        this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);
        myAnim.SetTrigger("Attack");
        while (myEnemy != null)
        {
            chill -= Time.deltaTime;
            if(chill < 0.0f)
            {
                myAgent.SetDestination(transform.position);
                this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);
                myAnim.SetTrigger("Attack");
                chill = monStat.orgData.attackSpeed;
            }

            targetDir = myTarget.transform.position - this.transform.position;
            targetDist = targetDir.magnitude; // 공격하는 중 타겟의 거리 계산
            if (targetDist > monStat.orgData.strikingDist) // 타겟이 범위를 벗어났다면?
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
        // 난수를 생성해서 랜덤하게 아이템을 switch 로 드랍되도록 만들어보자
        GameObject DropItem = Instantiate(monStat.orgData.DropItems[0].obj, this.transform.position, Quaternion.identity) as GameObject; // 드랍 아이템
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

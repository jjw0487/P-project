using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public struct MonsterStat
{
    public MonData orgData;
    public float curHP;
}

public class Monster : CharacterProperty
{
    //mutual data
    [Header("Monster Data")]
    public MonsterStat monStat;
    public Transform myAttakPoint;
    public enum MonsterState
    {
        Create, Idle, Roam, Target, Attack, Dead
    }
    public MonsterState state;
    public bool isDead;
    protected Vector3 roamPos;
    protected Vector3 IdlePos;

    [Header("Target")]
    public Transform myTarget = null;
    public LayerMask enemyMask;
    public Player myEnemy = null;

    protected Vector3 targetPos;
    protected Vector3 targetDir;
    protected float targetDist;

    protected bool onBattle;

    protected Coroutine c = null; //중복실행 방지
    protected Coroutine idle = null; //중복실행 방지
    protected Coroutine roam = null; //중복실행 방지
    protected Coroutine attack = null; //중복실행 방지
    protected Coroutine isAlive = null;

    [Header("UI")]
    protected GameObject hpObj;
    protected GameObject exObj;
    protected GameObject floatingDmg;
    protected Slider HPSlider;

    public Transform myHpPos;
    public Transform myDmgPos;
 
    public virtual void ChangeState(MonsterState what)
    {
        if (state == what) { return; }
        state = what;
        switch (state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                onBattle = false;
                myAgent.SetDestination(IdlePos);
                StartCoroutine(IfAlive());
                roam = StartCoroutine(DelayState(MonsterState.Roam, 5f));
                break;
            case MonsterState.Roam:
                Vector3 rndPos = Vector3.zero;
                rndPos.x = UnityEngine.Random.Range(-2.5f, 2.5f);
                rndPos.z = UnityEngine.Random.Range(-2.5f, 2.5f);
                roamPos = transform.position + rndPos;
                myAgent.SetDestination(roamPos);
                idle = StartCoroutine(DelayState(MonsterState.Idle, 5f));
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
                if(myEnemy != null)myEnemy.GetEXP(monStat.orgData.exp);
                SceneData.Inst.interactableUIManager.SetGold(monStat.orgData.currency);
                if (hpObj != null) Destroy(hpObj);
                hpObj = null;
                StartCoroutine(DelayDead(4f));
                break;
        }
    }

    public virtual void StateProcess()
    {
        switch (state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                if (myAgent.remainingDistance > 0.2f) { myAnim.SetBool("IsRunning", true); }
                else { myAnim.SetBool("IsRunning", false); }
                break;
            case MonsterState.Roam:
                if (myAgent.remainingDistance > 0.2f) { myAnim.SetBool("IsRunning", true); }
                else { myAnim.SetBool("IsRunning", false); }
                break;
            case MonsterState.Target:
                if (myTarget != null)
                {
                    targetDir = myTarget.transform.position - this.transform.position;
                    targetDist = targetDir.magnitude;
                    targetPos = myTarget.transform.position;

                    if (!myAgent.pathPending) { myAgent.SetDestination(targetPos); }

                    if (targetDist <= monStat.orgData.strikingDist) { ChangeState(MonsterState.Attack); }

                    if (Vector3.Distance(myTarget.transform.position, this.transform.position) > 10.0f) { OnExitMotion(); }
                }
                else { ChangeState(MonsterState.Idle); }

                if (myAgent.remainingDistance > 0.2f) { myAnim.SetBool("IsRunning", true); }
                else { myAnim.SetBool("IsRunning", false); }

                break;
            case MonsterState.Attack:
                if (myAgent.remainingDistance > 0.2f) { myAnim.SetBool("IsRunning", true); }
                else { myAnim.SetBool("IsRunning", false); }
                break;
            case MonsterState.Dead:
                break;
        }
    }

    protected virtual void Start()
    {
        IdlePos = this.transform.position;
        isDead = false;
        ChangeState(MonsterState.Idle);
        monStat.curHP = monStat.orgData.HP;
        myAgent.speed = monStat.orgData.agentSpeed;
        myAgent.stoppingDistance = monStat.orgData.agentStopDist;
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //behaviorstate -> exit 할 때
        if (!isDead)
        {
            if ((enemyMask & 1 << other.gameObject.layer) != 0)
            {
                //
                onBattle = true;
                if (hpObj == null)
                {
                    exObj = Instantiate(Resources.Load("UI/ExMark")) as GameObject;
                    hpObj = Instantiate(Resources.Load("UI/MonHP"), SceneData.Inst.HPBars) as GameObject;
                    hpObj.GetComponent<MonsterHP>().myTarget = myHpPos;
                    exObj.GetComponent<ExclamationMark>().myTarget = myDmgPos;
                    hpObj.transform.localScale = monStat.orgData.HPlocalScale;
                    exObj.transform.localScale = monStat.orgData.HPlocalScale * 0.5f;
                    HPSlider = hpObj.GetComponent<MonsterHP>().myBar;
                }
                //
                if (myTarget == null) { myTarget = other.transform.parent.GetComponent<Transform>(); }
                if (myEnemy == null) { myEnemy = other.transform.parent.GetComponent<Player>(); }
                ChangeState(MonsterState.Target);
            }
        }
    }


    public void OnExitMotion()
    {
        myTarget = null;
        myEnemy = null;
        //
        Destroy(hpObj);
        hpObj = null;
        //
        if (!isDead)
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
                myEnemy.OnDmg(monStat.orgData.atk);
                break;
            }
            if (col.name == "Broom")
            {
                myEnemy.OnDmg(monStat.orgData.atk);
                break;
            }
            if (col.name == "Guoba2(Clone)")
            {
                myTarget.GetComponent<Guoba_M>().OnDmg(monStat.orgData.atk);
                break;
            }
        }
    }

    public virtual void OnDamage(float dmg)
    {
        myAgent.SetDestination(transform.position);
        float damage = dmg - monStat.orgData.dp;
        float dmgRndVal = UnityEngine.Random.Range(damage * 0.7f, damage);
        if (damage < 0) { dmgRndVal = 0; }
        int finalDmg = (int)dmgRndVal;
        monStat.curHP -= finalDmg;
        floatingDmg = Instantiate(Resources.Load("UI/Dmg"), SceneData.Inst.FloatingDmg) as GameObject;
        floatingDmg.GetComponent<FloatingDamage>().myPos = myDmgPos;
        floatingDmg.GetComponent<FloatingDamage>().dmg.text = finalDmg.ToString();

        if (monStat.curHP < 0.0f) ChangeState(MonsterState.Dead);
        else myAnim.SetTrigger("IsHit");
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

    protected virtual IEnumerator Attacking(float chill)
    {

        myAgent.SetDestination(transform.position);

        float cool = 0.3f; // 공격을 시작 전 0.3 만큼의 대기
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }

        if (myTarget != null)
        {
            this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);
            myAnim.SetTrigger("Attack");
        }
        else { ChangeState(MonsterState.Idle); yield break; }

        while (myTarget != null)
        {
            chill -= Time.deltaTime;
            if (chill < 0.0f)
            {
                myAgent.SetDestination(transform.position);
                this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);
                myAnim.SetTrigger("Attack");
                chill = monStat.orgData.attackSpeed; //공격 속도
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

    protected IEnumerator DelayState(MonsterState state, float chill)
    {
        yield return new WaitForSeconds(chill);
        ChangeState(state);
    }

    protected virtual IEnumerator DelayDead(float chill)
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
        GameObject DropItem = Instantiate(monStat.orgData.dropItems[0].obj, this.transform.position + new Vector3(0f,2f,0f), Quaternion.identity); // 드랍 아이템
        DropItem.transform.SetParent(SceneData.Inst.ItemPool);
        Destroy(gameObject);
    }

    protected IEnumerator Debuff(float chill, float percents)
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

    protected IEnumerator IfAlive()
    {
        while(true)
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
            yield return null;
        }
    }
}

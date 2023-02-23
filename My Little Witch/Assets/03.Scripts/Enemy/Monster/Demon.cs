using System.Collections;
using UnityEngine;

public class Demon : Monster
{
    [SerializeField] private Transform[] atkPoint;
    [SerializeField] private GameObject[] eftObj;
    private int rndNum;
    private float atkDist;
    public TrailRenderer tr;


    protected override void Start()
    {
        base.Start();
        rndNum = 0;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myAnim.SetTrigger("OnBattle");
        }


    }

    public override void ChangeState(MonsterState what)
    {
        // 보스몬스터
        // 1. 로밍이 없다.
        // 2. 범위가 멀어졌다고 타겟 null이 되지 않는다.
        // 3. 랜덤 어택을 갖는다. (랜덤어택은 단거리, 중거리, 장거리 마법이 존재)
        // 4.
        if (state == what) return;
        state = what;
        switch (state)
        {
            case MonsterState.Create:
                if (isAlive == null) isAlive = StartCoroutine(IfAlive());
                ChangeState(MonsterState.Idle);
                break;
            case MonsterState.Idle:
                onBattle = false;
                StartCoroutine(IfAlive());
                myAgent.SetDestination(IdlePos);
                //roam = StartCoroutine(DelayState(MonsterState.Roam, 5f));
                break;
            case MonsterState.Roam:
                //Vector3 rndPos = Vector3.zero;
                //rndPos.x = UnityEngine.Random.Range(-2.5f, 2.5f);
                //rndPos.z = UnityEngine.Random.Range(-2.5f, 2.5f);
                //roamPos = transform.position + rndPos;
                //myAgent.SetDestination(roamPos);
                //idle = StartCoroutine(DelayState(MonsterState.Idle, 5f));
                break;
            case MonsterState.Target:
                CreateRandomNumber();
                //if (idle != null) { StopCoroutine(idle); idle = null; }
                //if (roam != null) { StopCoroutine(roam); roam = null; }
                if (attack != null) { StopCoroutine(attack); attack = null; print("중단"); }
                break;
            case MonsterState.Attack:
                myAgent.SetDestination(transform.position);
                attack = StartCoroutine(Attacking()); //코루틴을 시작하며, 동시에 저장한다.         
                // StartCoroutine(EnemyCheck());
                break;
            case MonsterState.Dead:
                StopAllCoroutines();
                myEnemy.GetEXP(monStat.orgData.exp);
                SceneData.Inst.interactableUIManager.SetGold(monStat.orgData.currency);
                Destroy(hpObj);
                hpObj = null;
                StartCoroutine(DelayDead(4f));
                break;
        }
    }

    public override void StateProcess()
    {
        switch (state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                //if (myAgent.remainingDistance > 0.2f) { myAnim.SetBool("IsRunning", true); }
                //else { myAnim.SetBool("IsRunning", false); }
                break;
            case MonsterState.Roam:
                //if (myAgent.remainingDistance > 0.2f) { myAnim.SetBool("IsRunning", true); }
                //else { myAnim.SetBool("IsRunning", false); }
                break;
            case MonsterState.Target:
                if (myTarget != null)
                {
                    targetDir = myTarget.transform.position - this.transform.position;
                    targetDist = targetDir.magnitude;
                    targetPos = myTarget.transform.position;

                    if (!myAgent.pathPending) { myAgent.SetDestination(targetPos); }

                    if (rndNum == 0) { if (targetDist <= monStat.orgData.strikingDist) { ChangeState(MonsterState.Attack); } }
                    if (rndNum == 1) { if (targetDist <= 3f) { ChangeState(MonsterState.Attack); } } //스킬마다 다른 어택 범위를 갖는다.
                    if (rndNum == 2) { if (targetDist <= 10f) { ChangeState(MonsterState.Attack); } }

                    if (Vector3.Distance(myTarget.transform.position, this.transform.position) > 20.0f) { OnExitMotion(); }
                    // 20.0f 으로 재정의
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
    public void DemonAtk1()
    {
        Collider[] hitColliders = Physics.OverlapSphere(atkPoint[0].position, 2f);

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

    public void DemonAtk2()
    {
        Collider[] hitColliders = Physics.OverlapSphere(atkPoint[1].position, 2f);

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

    public void DemonAtk3()
    {
        GameObject obj = Instantiate(eftObj[0], atkPoint[2].position, Quaternion.identity);
        obj.GetComponent<DemonAttack>().SetTarget(myTarget);
    }



    void CreateRandomNumber()
    {
        rndNum = Random.Range(0, 3); // to get value from 0 to 2

        if (rndNum == 0) { atkDist = monStat.orgData.strikingDist; }
        if (rndNum == 1) { atkDist = 5f; }
        if (rndNum == 2) { atkDist = 7f; }
    }
    public override void OnDamage(float dmg)
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

        if (monStat.curHP < 0.0f)
        {
            ChangeState(MonsterState.Dead);
        }
        else if (finalDmg > 30f) // 30f 일 때 실행될 수 있도록 재정의
        {
            myAnim.SetTrigger("IsHit");
        }
    }

    protected IEnumerator Attacking()
    {
        myAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(1.0f);
        float chill = 0f;
        if (myTarget == null) { ChangeState(MonsterState.Idle); yield break; } // while문 전에 한 번 더 검사
        while (myTarget != null)
        {
            chill -= Time.deltaTime;
            if (chill < 0.0f)
            {
                myAgent.SetDestination(transform.position);
                this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);
                if (rndNum == 0) { myAnim.SetTrigger("Attack"); }
                if (rndNum == 1) { myAnim.SetTrigger("Attack2"); }
                if (rndNum == 2) { myAnim.SetTrigger("Attack3"); }
                chill = monStat.orgData.attackSpeed; //공격 속도
            }
            this.transform.rotation = Quaternion.LookRotation((myTarget.position - transform.position).normalized);
            targetDir = myTarget.transform.position - this.transform.position;
            targetDist = targetDir.magnitude; // 공격하는 중 타겟의 거리 계산
            if (targetDist > atkDist) // 타겟이 범위를 벗어났다면?
            {
                ChangeState(MonsterState.Target);
                yield break;
            }
            yield return null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static Movement;

public class Monster : MonoBehaviour
{
    public enum MonsterState
    {
        Create, Idle, Roam, Target, Attack, Dead
    }

    public MonsterState state = MonsterState.Create;
    public Transform target;
    Movement myEnemy;
    public LayerMask enemyMask;
    Vector3 targetDir;
    Animator myAnim;
    NavMeshAgent monAgent;
    Vector3 roamPos;
    public void ChangeState(MonsterState what)
    {
        //if (state == what) return;
        state = what;
        switch(state)
        {
            case MonsterState.Create:
                break;
            case MonsterState.Idle:
                print("Idle");
                StartCoroutine(DelayRoaming(5f));
                break;
            case MonsterState.Roam:
                print("Roam");
                Vector3 rndPos = Vector3.zero;
                rndPos.x = Random.Range(-1.0f, 1.0f);
                rndPos.z = Random.Range(-1.0f, 1.0f);
                roamPos = transform.position + rndPos;
                monAgent.SetDestination(roamPos);
                ChangeState(MonsterState.Idle);
                break;
            case MonsterState.Target:
                break;
            case MonsterState.Attack:
                break;
            case MonsterState.Dead:
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
                break;
            case MonsterState.Target:/*
                targetDir = target.transform.position;
                monAgent.SetDestination(targetDir);
                if (monAgent.remainingDistance > 0.1f)
                {
                    myAnim.SetBool("IsRunning", true);
                }*/

                /*if (monAgent.remainingDistance < 0.1f)
                {
                    myAnim.SetBool("IsRunning", false);
                    //state = MonsterState.Attack;
                    //ChangeState(MonsterState.Attack);
                    //myAnim.SetTrigger("Attack"); // 이러면 어택을 계속하겠지? 정우야?
                }*/
                    break;
            case MonsterState.Attack:
                break;
            case MonsterState.Dead:
                break;
        }
    }
    private void Start()
    {
        monAgent = this.GetComponent<NavMeshAgent>();
        ChangeState(MonsterState.Idle);
        myAnim = this.GetComponent<Animator>();

    }

    private void Update()
    {
        if(monAgent.remainingDistance > 0.1f)
        {
            myAnim.SetBool("IsRunning", true);
        }
        else
        {
            myAnim.SetBool("IsRunning", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myEnemy = other.transform.parent.GetComponent<Movement>();
            ChangeState(MonsterState.Target);
            print("Target");
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (state == MonsterState.Target && (enemyMask & 1 << other.gameObject.layer) != 0)
        {
            targetDir = target.transform.position;
            monAgent.SetDestination(targetDir);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print("Exit");
        //monAgent.SetDestination(transform.position);
        
    }

    IEnumerator Attacking()
    {
        myAnim.SetTrigger("Attack");
        if (monAgent.remainingDistance > 0.1f)
        {
            ChangeState(MonsterState.Target);
            yield return null;
        }
        yield return new WaitForSeconds(3f);
    }

    IEnumerator DelayRoaming(float chill)
    {
        yield return new WaitForSeconds(chill);
        ChangeState(MonsterState.Roam);
    }




}

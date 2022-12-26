using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    Vector3 IdlePos;
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
                StartCoroutine(DelayRoaming(3f));
                monAgent.SetDestination(IdlePos);
                break;
            case MonsterState.Roam:
                print("Roam");
                Vector3 rndPos = Vector3.zero;
                rndPos.x = Random.Range(-1.5f, 1.5f);
                rndPos.z = Random.Range(-1.5f, 1.5f);
                roamPos = transform.position + rndPos;
                monAgent.SetDestination(roamPos);
                StartCoroutine(DelayIdle(3f));
                break;
            case MonsterState.Target:
                StopAllCoroutines();
                break;
            case MonsterState.Attack:
                StopAllCoroutines();
                monAgent.SetDestination(transform.position);
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
                if (monAgent.remainingDistance > 0.1f)
                {
                    myAnim.SetBool("IsRunning", true);
                }
                else
                {
                    myAnim.SetBool("IsRunning", false);
                }
                break;
            case MonsterState.Roam:
                if (monAgent.remainingDistance > 0.1f)
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
                    monAgent.SetDestination(targetDir);
                }
                if (monAgent.remainingDistance > 0.1f)
                {
                    myAnim.SetBool("IsRunning", true);
                }
                else
                {
                    myAnim.SetBool("IsRunning", false);
                }
                break;
            case MonsterState.Attack:
                if (monAgent.remainingDistance > 0.1f)
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
    private void Start()
    {
        monAgent = this.GetComponent<NavMeshAgent>();
        ChangeState(MonsterState.Idle);
        myAnim = this.GetComponent<Animator>();
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
            print("Target");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myEnemy = null;
        ChangeState(MonsterState.Idle);
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

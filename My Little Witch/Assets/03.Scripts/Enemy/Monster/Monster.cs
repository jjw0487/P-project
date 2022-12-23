using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Movement;

public class Monster : MonoBehaviour
{
    public enum MonsterState
    {
        Idle, Target, Attack, Dead
    }

    public MonsterState state;
    public Transform target;
    Movement myEnemy;
    public LayerMask enemyMask;
    Vector3 targetDir = Vector3.zero;
    Animator myAnim;
    NavMeshAgent monAgent;

    public void ChangeState(MonsterState what)
    {
        //if (state == what) return;

        switch(state)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Target:
                break;
            case MonsterState.Attack:
                myAnim.SetTrigger("Attack");
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
            case MonsterState.Idle:
                break;
            case MonsterState.Target:
                targetDir = target.transform.position - this.transform.position;

                monAgent.SetDestination(targetDir);

                if (monAgent.remainingDistance > 0.3f)
                {
                    myAnim.SetBool("IsRunning", true);
                }

                if (monAgent.remainingDistance < 0.3f)
                {
                    myAnim.SetBool("IsRunning", false);
                    state = MonsterState.Attack;
                    ChangeState(MonsterState.Attack);
                    //myAnim.SetTrigger("Attack"); // 이러면 어택을 계속하겠지? 정우야?
                }
                    break;
            case MonsterState.Attack:
                if (monAgent.remainingDistance > 0.3f)
                {
                    state = MonsterState.Target;
                    ChangeState(MonsterState.Target);
                }
                break;
            case MonsterState.Dead:
                break;
        }
    }
    private void Start()
    {
        monAgent = this.GetComponent<NavMeshAgent>();
        state = MonsterState.Idle;
        myAnim = this.GetComponent<Animator>();

    }

    private void Update()
    {
        StateProcess();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            print("확인");
            myEnemy = other.transform.parent.GetComponent<Movement>();
            state = MonsterState.Target;
            ChangeState(MonsterState.Target);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        myEnemy = null;
        if(myEnemy == null)
        {
            ChangeState(MonsterState.Idle);
        }
    }



   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public Movement myEnemy;

    public Transform myNeck;
    public Transform myArrowStand;
    public GameObject orgArrow;
    //public Collider mySensor;
    public LayerMask enemyMask;
    public Transform myTarget;

    private void OnTriggerEnter(Collider other)
    {
        
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myEnemy = other.transform.parent.GetComponent<Movement>();
            StartCoroutine(Targetting());
            StartCoroutine(Attacking(2f, 3f));
        }

    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        myEnemy = null;
        StopAllCoroutines();
    }


    public IEnumerator Targetting()
    {
        while (myEnemy != null)
        {

            print("Target");
            // 3차원 회전을 간단하게 처리하는 방법
            Quaternion rot = Quaternion.LookRotation((myEnemy.AttackMark.position - myArrowStand.position).normalized);
            myNeck.rotation = Quaternion.Slerp(myNeck.rotation, rot, Time.deltaTime * 10f);

            yield return null;
        }
    }

    IEnumerator Attacking(float cool, float attackSpeed)
    {
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }

        while (myEnemy != null)
        {

            print("Attack");
            GameObject obj = Instantiate(orgArrow, myArrowStand.position, myArrowStand.rotation);
            obj.GetComponent<Arrow>().OnFire(myEnemy);
            yield return new WaitForSeconds(attackSpeed);
           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public Movement myEnemy = null;

    public Transform myNeck;
    public Transform myArrowStand;
    public GameObject orgArrow;
    //public Collider mySensor;
    public LayerMask enemyMask;
    public Transform myTarget;

    private void OnTriggerEnter(Collider other)
    {
        myEnemy = other.GetComponent<Movement>();

        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            StartCoroutine(Targetting());
            StartCoroutine(Attacking(2f, 3f));
        }

        //StartCoroutine(Targetting());
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
            GameObject obj = Instantiate(orgArrow, myArrowStand.position, myArrowStand.rotation);
            obj.GetComponent<Arrow>().OnFire(myEnemy, enemyMask);
            yield return new WaitForSeconds(attackSpeed);
           
        }
    }
}

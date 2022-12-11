using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public Movement myEnemy = null;

    public Transform myNeck;
    public Transform myArrowStand;
    public GameObject orgArrow;
    public Collider mySensor;
    public LayerMask enemyMask;

    private void OnTriggerEnter(Collider other)
    {
        myEnemy = other.GetComponent<Movement>();

        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            StartCoroutine(Targetting());
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
        Attacking();
    }


    public IEnumerator Targetting()
    {
        
        while (myEnemy != null)
        {
           
            // 3차원 회전을 간단하게 처리하는 방법
            Quaternion rot = Quaternion.LookRotation((myEnemy.AttackMark.position - myNeck.position).normalized);
            myNeck.rotation = Quaternion.Slerp(myNeck.rotation, rot, Time.deltaTime * 10f);

            yield return null;
        }
    }

    IEnumerator Attacking()
    {
        while (myEnemy != null)
        {
            GameObject obj = Instantiate(orgArrow, myArrowStand.position, Quaternion.identity);
            //obj.GetComponent<Projectile>().OnFire(myEnemy, enemyMask, myStat.AttackPoint);
            
            yield return new WaitForSeconds(1f); // 업그레이드로 바뀌었을 때 전달받을 수 있도록
            Destroy(obj);
        }
    }
}

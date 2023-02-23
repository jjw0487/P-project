using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Player myEnemy;
    [SerializeField] private Transform myNeck;
    [SerializeField] private Transform myArrowStand;
    [SerializeField] private GameObject orgArrow;
    public LayerMask enemyMask;

    private void OnTriggerEnter(Collider other)
    {

        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            myEnemy = other.transform.parent.GetComponent<Player>();
            StartCoroutine(Targetting());
            StartCoroutine(Attacking(2f, 3f));
        }
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
            Quaternion rot = Quaternion.LookRotation((myEnemy.transform.position + new Vector3(0f, 1f, 0f) - myArrowStand.position).normalized);
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
            obj.GetComponent<Arrow>().OnFire(myEnemy);
            yield return new WaitForSeconds(attackSpeed);

        }
    }
}

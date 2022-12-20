using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    Movement myTarget = null;
    private void OnTriggerEnter(Collider other)
    {
        myTarget.OnDmg(20f);
        Destroy(gameObject);
    }


    public void OnFire(Movement target)
    {
        myTarget = target;
        StartCoroutine(Attacking());
    }
    public IEnumerator Attacking()
    {
        Vector3 target = myTarget.AttackMark.position;
        Vector3 pos = this.transform.position;
        Vector3 dir = target - pos;
        float totalDist = dir.magnitude;
        dir.Normalize();
        while(totalDist > 0f)
        {
            print("fire");
            float delta = speed * Time.deltaTime;
            if(totalDist < delta)
            {
                delta = totalDist;
            }
            totalDist -= delta;
            this.transform.position += dir * delta;

            yield return null;
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow : MonoBehaviour
{

    public IEnumerator Attacking(Movement myTarget)
    {
        print("fire");
        Vector3 pos = this.transform.position;
        Vector3 dir = myTarget.AttackMark.position - pos;
        float totalDist = dir.magnitude;
        //float radius = transform.localScale.x * 0.1f;

        float delta = 2f * Time.deltaTime;
        while (totalDist > 0f)
        {

            if (totalDist < delta)
            {
                delta = totalDist;
            }
            totalDist -= delta;
            this.transform.position += dir * delta;
            /*float delta = Time.fixedDeltaTime * 7f;
            if (myTarget != null)
            {
                pos = myTarget.AttackMark.position;
            }
            dir = pos - transform.position;
            if (delta > dir.magnitude)
            {
                delta = dir.magnitude;
            }
            dir.Normalize();*/
           /* if (myTarget != null)
            {
                Ray ray = new Ray(transform.position, dir);
                if (Physics.Raycast(ray, out RaycastHit hit, delta + radius, mask))
                {
                    transform.position = hit.point + -dir * radius;
                    Destroy(gameObject);
                    //myTarget.OnDamage(AttackPoint);
                    //OnHit();
                    yield break;
                }
            }*/
            //transform.Translate(dir * delta, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }
}

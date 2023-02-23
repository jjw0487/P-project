using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    Player myTarget = null;
    private void OnTriggerEnter(Collider other)
    {
        myTarget.OnDmg(20f);
        Destroy(gameObject);
    }
    public void OnFire(Player target)
    {
        myTarget = target;
        StartCoroutine(Attacking());
    }
    public IEnumerator Attacking()
    {
        Vector3 target = myTarget.transform.position + new Vector3(0f, 1.0f, 0f);
        Vector3 pos = this.transform.position;
        Vector3 dir = target - pos;
        float totalDist = dir.magnitude;
        dir.Normalize();
        while (totalDist > 0f)
        {
            float delta = speed * Time.deltaTime;
            if (totalDist < delta)
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

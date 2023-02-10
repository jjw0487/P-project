using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DemonAttack : ProjectileMover
{ // DemonAttack


    Vector3 dir;

    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, 5);
    }

    public void SetTarget(Transform target)
    {
        dir = (target.position - this.transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = dir * speed;
            //transform.position += targetDir * (speed * Time.deltaTime);
        }
    }


    protected override void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1.5f);
            foreach (Collider col in hitColliders)
            {
                if (col.name == "KIKI")
                {
                    col.GetComponentInParent<Player>().OnDmg(40f);
                    break;
                }
                if (col.name == "Broom")
                {
                    col.GetComponentInParent<Player>().OnDmg(40f);
                    break;
                }
                if (col.name == "Guoba2(Clone)")
                {
                    col.GetComponent<Guoba_M>().OnDmg(40f);
                    break;
                }
            }
        }

        base.OnCollisionEnter(collision);
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : ProjectileMover
{
    public SkillData skillData;
    public Vector3 targetDir;

    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, 3f);
    }

    public void SetTarget(Transform target, Vector3 offset)
    {
        targetDir = ((target.position + offset) - transform.position);
    }

    protected void FixedUpdate()
    {
        if (targetDir != Vector3.zero && speed != 0)
        {
            transform.position += targetDir * (speed * Time.deltaTime);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, skillData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                /*Monster mon = col.GetComponentInParent<Monster>();
                if (mon == null) continue;*/   //나중에 nullref 나오면 예외처리 해줘야함.
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(skillData.dmg[skillData.level-1] + SceneData.Inst.myPlayer.SP);
                }
            }
        }

        base.OnCollisionEnter(collision);
    }
}

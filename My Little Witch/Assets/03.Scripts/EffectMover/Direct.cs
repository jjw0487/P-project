using UnityEngine;

public class Direct : ProjectileMover
{

    public SkillData skillData;


    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, 3f);
    }


    void FixedUpdate()
    {
        if (speed != 0) { rb.velocity = transform.forward * speed; }
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
                    col.GetComponentInParent<Monster>().OnDamage(skillData.dmg[skillData.level]);
                }
            }
        }
        base.OnCollisionEnter(collision);
    }
}

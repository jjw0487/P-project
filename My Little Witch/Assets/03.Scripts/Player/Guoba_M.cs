using System.Collections;
using UnityEngine;
using static Monster;

public class Guoba_M : MonoBehaviour
{
    public enum GuobaMachineState { Create, Target, Attack, Delay, Destroy }
    public GuobaMachineState GuobaState = GuobaMachineState.Create;

    public SkillData myData;

    [SerializeField] private float HP;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject appearance;
    private Transform previousTarget = null;

    void Start()
    {
        Gu_ChangeState(GuobaMachineState.Create); // 업데이트 없이 만들어보자
        previousTarget = SceneData.Inst.myPlayer.GetComponent<Transform>();
        HP = myData.debuffTime[myData.level - 1];
    }

    public void Gu_ChangeState(GuobaMachineState what)
    {
        GuobaState = what;
        switch (GuobaState)
        {
            case GuobaMachineState.Create:
                Instantiate(appearance, this.transform.position, Quaternion.identity);
                Gu_ChangeState(GuobaMachineState.Target);
                break;
            case GuobaMachineState.Target:
                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, myData.overlapRadius);
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Monster")) // 검출된 콜라이더 중에 몬스터가 있다면
                    {
                        if (!col.GetComponentInParent<Monster>().isDead) //죽지 않았다면
                        {
                            col.GetComponentInParent<Monster>().myTarget = this.transform;
                            col.GetComponentInParent<Monster>().ChangeState(MonsterState.Target);
                        }
                    }
                }
                StartCoroutine(BeforeDestroy(myData.percentage[myData.level - 1])); // destroy time
                break;
            case GuobaMachineState.Attack:
                break;
        }
    }

    public void OnDmg(float dmg)
    {
        HP -= dmg;
        if (HP <= 0f)
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, myData.overlapRadius);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Monster")) // 검출된 콜라이더 중에 몬스터가 있다면
                {
                    if (!col.GetComponentInParent<Monster>().isDead) //죽지 않았다면
                    {
                        if (Vector3.Distance(col.GetComponentInParent<Transform>().position,
                            previousTarget.position) < 10f)
                        {
                            col.GetComponentInParent<Monster>().myTarget = previousTarget;
                            col.GetComponentInParent<Monster>().ChangeState(MonsterState.Target);
                        }
                        else
                        {
                            col.GetComponentInParent<Monster>().myTarget = null;
                        }
                    }
                }
            }
            Explosion();
            Destroy(gameObject);
        }
    }

    IEnumerator BeforeDestroy(float howlong)
    {
        while (howlong > 0f)
        {
            howlong -= Time.deltaTime;
            yield return null;
        }

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, myData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster")) // 검출된 콜라이더 중에 몬스터가 있다면
            {
                if (!col.GetComponentInParent<Monster>().isDead) //죽지 않았다면
                {
                    if (Vector3.Distance(col.GetComponentInParent<Transform>().position, previousTarget.position) < 10f)
                    {
                        col.GetComponentInParent<Monster>().myTarget = previousTarget;
                        col.GetComponentInParent<Monster>().ChangeState(MonsterState.Target);
                    }
                    else
                    {
                        col.GetComponentInParent<Monster>().myTarget = null;
                    }
                }
            }
        }
        Explosion();
        Destroy(gameObject);
    }

    void Explosion()
    {
        Instantiate(explosion, this.transform.position, Quaternion.identity);

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 2f);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(myData.debuffTime[myData.level - 1]); // 폭발 데미지
                }
            }
        }
    }
}

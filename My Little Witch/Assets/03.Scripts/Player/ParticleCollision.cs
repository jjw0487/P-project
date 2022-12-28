using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

[Serializable]
public struct SkillStat
{
    public SkillData orgData;
}

public class ParticleCollision : MonoBehaviour
{
    public SkillStat skillStat;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) { Instantiate(skillStat.orgData.Effect, this.transform.position, Quaternion.identity); }
        //�ϴ� �̰� �Ǵ°� Ȯ��
    }

    public void OverlapCol()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, skillStat.orgData.overlapRadius);

        foreach (Collider col in hitColliders)
        {
            /*if (col.name == "")
            {
                myEnemy.OnDmg(skillStat.orgData.dmg);
                break;
            }
            if (col.name == "")
            {
                myEnemy.OnDmg(monStat.orgData.AT);
                break;
            }*/

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGuy : Monster
{
    // ��������, ���� �� ī��Ʈ�� �Ͽ� �����ǰ� ����Ʈ�� �����Ѵ�.


    protected override IEnumerator DelayDead(float chill)
    {
        MonsterSpawner.Inst.KilledMonsterCounter(this.monStat.orgData.monsterName, this.gameObject);
        myAgent.SetDestination(transform.position);
        myAnim.SetTrigger("Death");
        isDead = true;
        while (chill > 0.0f)
        {
            chill -= Time.deltaTime;
            yield return null;
        }
        // ������ �����ؼ� �����ϰ� �������� switch �� ����ǵ��� ������
        GameObject DropItem = Instantiate(monStat.orgData.dropItems[0].obj, this.transform.position + Vector3.up, Quaternion.identity); // ��� ������
        Destroy(gameObject);
    }
}

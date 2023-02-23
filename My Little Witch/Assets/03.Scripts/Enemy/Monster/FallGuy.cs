using System.Collections;
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
        int rndNum = UnityEngine.Random.Range(0, 11); // ������ �����ؼ� ������ �����ϰ� ���
        if (rndNum < 5 && monStat.orgData.dropItems[0] != null)
        {
            GameObject DropItem = Instantiate(monStat.orgData.dropItems[0].obj,
                this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity); // ��� ������
            DropItem.transform.SetParent(SceneData.Inst.ItemPool);
        }
        else if (rndNum == 1 && monStat.orgData.dropItems[0] != null)
        {
            GameObject DropItem = Instantiate(monStat.orgData.dropItems[1].obj,
                this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity); // ��� ������
            DropItem.transform.SetParent(SceneData.Inst.ItemPool);
        }
        Destroy(gameObject);
    }
}

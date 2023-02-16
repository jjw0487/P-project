using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGuy : Monster
{
    // 마을몬스터, 죽을 때 카운트를 하여 스폰되고 퀘스트를 진행한다.


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
        // 난수를 생성해서 랜덤하게 아이템을 switch 로 드랍되도록 만들어보자
        GameObject DropItem = Instantiate(monStat.orgData.dropItems[0].obj, this.transform.position + Vector3.up, Quaternion.identity); // 드랍 아이템
        Destroy(gameObject);
    }
}

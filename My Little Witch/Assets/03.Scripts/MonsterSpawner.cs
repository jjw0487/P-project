using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Events;
using UnityEditor.PackageManager.Requests;

public class MonsterSpawner : MonoBehaviour
{
    //업데이트 문 없이 몬스터에서 몬스터가 죽을 때 counter를 증가시켜 몬스터 생성
    public static MonsterSpawner Inst = null;

    public int D_foxCounter;
    public int D_fallguyCounter;

    public int initialMonCounter;
    private int monCounter;

    public GameObject[] monsters;
    public Transform[] spawnPos;
    public Coroutine spawn;

    private QuestData curQuestData;

    List<GameObject> aliveFoxes = new List<GameObject> ();
    List<GameObject> aliveFallguys = new List<GameObject> ();

    public Action<string, int> addedQuest;

    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        InvokeRepeating("spawnInitialMonsters", 0, 1f);
        monCounter = 5;
    }
    public void spawnInitialMonsters()
    {
        if (--initialMonCounter <= 0) { CancelInvoke("spawnInitialMonsters"); }

        GameObject fallguyObj = Instantiate(monsters[1], spawnPos[1].position + new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        fallguyObj.transform.SetParent(this.transform);
        aliveFallguys.Add(fallguyObj);

        GameObject foxObj = Instantiate(monsters[0], spawnPos[0].position +  new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        foxObj.transform.SetParent(this.transform);
        aliveFoxes.Add(foxObj);
    }

    public void spawnFallguy()
    {
        if (--monCounter <= 0) { CancelInvoke("spawnFallguy"); monCounter = 5; }

        GameObject fallguyObj = Instantiate(monsters[1], spawnPos[1].position + new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        aliveFallguys.Add(fallguyObj);
        fallguyObj.transform.SetParent(this.transform);

    }

    public void spawnFox()
    {
        if (--monCounter <= 0) { CancelInvoke("spawnFox"); monCounter = 5; }

        GameObject foxObj = Instantiate(monsters[0], spawnPos[0].position + new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        aliveFoxes.Add(foxObj);
        foxObj.transform.SetParent(this.transform);
    }

    public void KilledMonsterCounter(string name, GameObject obj) //<= 몬스터에서 준다.
    {
        if (name == "Fox")
        {
            //D_foxCounter++;
            if (addedQuest != null) AddedQuestKilledMonsterCounter(name);
            aliveFoxes.Remove(obj);
            if(aliveFoxes.Count < 5)
            {
                InvokeRepeating("spawnFox", 0, 1f);
            }
        }
        else if(name == "Fallguy")
        {
            //D_fallguyCounter++;
            if (addedQuest != null) AddedQuestKilledMonsterCounter(name);
            aliveFallguys.Remove(obj);
            if (aliveFallguys.Count < 5)
            {
                InvokeRepeating("spawnFallguy", 0, 1f);
            }
        }
    }


    public void GetQuestData(QuestData questData)
    {

    }

    public void AddedQuestKilledMonsterCounter(string name , int count = 0)
    {
        // 0217 오늘 학원에서 하자
        //if (name == "Fox") { D_foxCounter++; if (D_foxCounter >= count) Scen }  
        //if (name == "Fallguy") { D_fallguyCounter++; }
    }

}

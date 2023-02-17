using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Events;
using UnityEditor.PackageManager.Requests;
using UnityEngine.UIElements;
using Unity.VisualScripting;

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

    private QuestTab curQuestTabData;

    List<GameObject> aliveFoxes = new List<GameObject> ();
    List<GameObject> aliveFallguys = new List<GameObject> ();


    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        InvokeRepeating("MS_spawnInitialMonsters", 0, 1f);
        monCounter = 5;
        curQuestTabData = null;
    }
    public void MS_spawnInitialMonsters()
    {
        if (--initialMonCounter <= 0) { CancelInvoke("MS_spawnInitialMonsters"); }

        GameObject fallguyObj = Instantiate(monsters[1], spawnPos[1].position + new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        fallguyObj.transform.SetParent(this.transform);
        aliveFallguys.Add(fallguyObj);

        GameObject foxObj = Instantiate(monsters[0], spawnPos[0].position +  new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        foxObj.transform.SetParent(this.transform);
        aliveFoxes.Add(foxObj);
    }

    public void MS_spawnFallguy()
    {
        if (--monCounter <= 0) { CancelInvoke("MS_spawnFallguy"); monCounter = 5; }

        GameObject fallguyObj = Instantiate(monsters[1], spawnPos[1].position + new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        aliveFallguys.Add(fallguyObj);
        fallguyObj.transform.SetParent(this.transform);

    }

    public void MS_spawnFox()
    {
        if (--monCounter <= 0) { CancelInvoke("MS_spawnFox"); monCounter = 5; }

        GameObject foxObj = Instantiate(monsters[0], spawnPos[0].position + new Vector3(UnityEngine.Random.Range(-15.0f, 15.0f), 0f, UnityEngine.Random.Range(-15.0f, 15.0f)), Quaternion.identity);
        aliveFoxes.Add(foxObj);
        foxObj.transform.SetParent(this.transform);
    }

    public void KilledMonsterCounter(string name, GameObject obj) //<= 몬스터에서 준다.
    {
        if (name == "Fox")
        {
            if (curQuestTabData != null && curQuestTabData.questData.questName.Contains("Fox Hunting"))
            {
                D_foxCounter++;
                MS_ReturnQuestProgress(D_foxCounter);
            }
            aliveFoxes.Remove(obj);
            if(aliveFoxes.Count < 5)
            {
                InvokeRepeating("MS_spawnFox", 0, 1f);
            }
        }
        else if(name == "Fallguy")
        {
            //D_fallguyCounter++;
            //if (curQuestTabData != null)
            aliveFallguys.Remove(obj);
            if (aliveFallguys.Count < 5)
            {
                InvokeRepeating("MS_spawnFallguy", 0, 1f);
            }
        }
    }

    public void MS_GetQuestData(QuestTab questData) // 퀘스트탭이 생성될 때 여기로 데이터를 전달
    {
        curQuestTabData = questData;
    }

    public void MS_ReturnQuestProgress(int count)
    {
        curQuestTabData.QT_GetProgressNum(count); // 퀘스트 화면에 보여줄 진행도
        
        if (curQuestTabData.questData.goalNumber <= D_foxCounter)
        {
            curQuestTabData.QT_GetSuccess();
            curQuestTabData = null; // 퀘스트 종료 후 null로 만들어 더이상 카운트 하지 않도록
        }
    }

}

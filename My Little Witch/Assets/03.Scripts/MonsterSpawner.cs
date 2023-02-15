using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int D_foxCounter;
    public int D_fallguyCounter;

    public int initialMonCounter;
    private int monCounter;

    public GameObject[] monsters;
    public Transform[] spawnPos;

    public Coroutine spawn;

    List<GameObject> aliveFoxes = new List<GameObject> ();
    List<GameObject> aliveFallguys = new List<GameObject> ();

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
            D_foxCounter++;
            aliveFoxes.Remove(obj);
            if(aliveFoxes.Count < 5)
            {
                print("fox");
                InvokeRepeating("spawnFox", 0, 1f);
            }
        }
        else if(name == "Fallguy")
        {
            D_fallguyCounter++;
            aliveFallguys.Remove(obj);
            if (aliveFallguys.Count < 5)
            {
                InvokeRepeating("spawnFallguy", 0, 1f);
            }
        }


    }

}

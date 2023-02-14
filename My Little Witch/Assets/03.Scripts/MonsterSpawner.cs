using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int foxCounter;
    public int fallguyCounter;

    public GameObject[] monsters;
    public Transform[] spawnPos;


    void Start()
    {
        
    }

    public void spawnMonsters()
    {
        if (--foxCounter == 0) CancelInvoke("spawnMonsters");
        Instantiate(monsters[0]);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingPaper : MonoBehaviour
{
    public const int maxCoolStack = 5;
    public const int minCoolStack = 0;

    public int coolStacks;

    void Start()
    {
        coolStacks = 0;
        StartCoroutine(StackingCoolStacks(5f));
    }

    void Update()
    {
        if(coolStacks > 0)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                coolStacks--;
                if (coolStacks == 4)
                {
                    StartCoroutine(StackingCoolStacks(5f));
                }
                print($"--{coolStacks}");
            }
        }
    }

    IEnumerator StackingCoolStacks(float cool)
    {
        float cooltime = cool;
        while(coolStacks < 5)
        {
            cool -= Time.deltaTime;
            if (cool <= 0.0f && coolStacks <= 5)
            {
                coolStacks++;
                print($"++{coolStacks}");
                cool = cooltime;
            }
            yield return null;
        }
        print($"full{coolStacks}");

    }
}

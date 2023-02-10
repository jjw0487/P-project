using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;


[Serializable]
public struct PlayerStat
{
    public PlayerData orgData;
    public float curHP;
    public float curMP;
}

public class Player : Movement
{
    [Header("Player")]
    [SerializeField] protected PlayerStat charStat;
    [SerializeField] protected int level; // 인스펙터에서 변경해서 테스트 위해
    [SerializeField] protected int curExp;
    [SerializeField] protected GameObject eagle;

    [Header("UI")]
    [SerializeField] private HPBar myHPBar;
    public Transform InteractionUIPos;


    protected override void Start()
    {
        base.Start();
        canRun = true; //시작할 때 바로 뛸 수 있도록
        level = charStat.orgData.Level;
        curExp = charStat.orgData.EXP[level - 1];
    }
    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetEXP(20);
        }
    }

    public void OnDmg(float dmg)
    {
        myHPBar.HandleHP(dmg);
        StartCoroutine(Stunned(0.7f));
        curAnim[0].SetTrigger("IsHit");
    }

    public void GetItemValue(int i, int value)
    {
        if (i == 1)
        {
            myHPBar.HandleHP(value);
        }
    }

    public void GetEXP(int exp)
    {
        curExp -= exp;
        if (curExp < 0)
        {
            curExp *= -1; //음수를 양수로
            //curExp *= -1;
            LevelUp(+curExp);
        }
    }
    public void LevelUp(int rest)
    {
        
        ++level; // 일단 스크립터블에 영향이 안가도록 데이터는 건드리지 말고 이렇게 두자
        SceneData.Inst.questManager.QM_GetPlayerLevel(level);
        curExp = charStat.orgData.EXP[level - 1];
        curExp -= rest;
        if (curExp < 0)
        {
            curExp *= -1;
            LevelUp(+curExp);
        }
        Instantiate(Resources.Load("Effect/MagicAura"), this.transform.position + Vector3.up * 0.2f, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
        GameObject obj = Instantiate(eagle, Camera.main.transform.position, Quaternion.identity);
        obj.GetComponent<EagleSpiral>().GetLevel(level);
    }
}

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
    [SerializeField] protected int level; // �ν����Ϳ��� �����ؼ� �׽�Ʈ ����
    [SerializeField] protected int curExp;
    [SerializeField] protected GameObject eagle;

    [Header("UI")]
    [SerializeField] private HPBar myHPBar;
    public Transform InteractionUIPos;


    protected override void Start()
    {
        base.Start();
        canRun = true; //������ �� �ٷ� �� �� �ֵ���
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
            curExp *= -1; //������ �����
            //curExp *= -1;
            LevelUp(+curExp);
        }
    }
    public void LevelUp(int rest)
    {
        
        ++level; // �ϴ� ��ũ���ͺ� ������ �Ȱ����� �����ʹ� �ǵ帮�� ���� �̷��� ����
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

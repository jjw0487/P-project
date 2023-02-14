using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

[Serializable]
public struct PlayerStat
{
    public PlayerData orgData;
}

public class Player : Movement
{

    [SerializeField]
    private int ValueType;
    public int valueType { get { return ValueType; } }


    [Header("Player")]
    [SerializeField] protected PlayerStat charStat;
    [SerializeField] protected int curExp;
    [SerializeField] protected GameObject eagle;

    [SerializeField]
    private int level; // ������ ������ �� setter�� private�� ���������
    public int Level { get { return level; } } // ���� <- ���� ���
    [SerializeField]
    private float curHP; // ����
    public float CurHP { get { return curHP; } } // ���� <- ���� ���
    [SerializeField] private float maxHP;
    [SerializeField]
    private float curMP; // ����
    public float CurMP { get { return curMP; } } // ����, ��ų�¿��� ����
    [SerializeField] private float maxMP;

    public Coroutine handleSlider; // skill���� ����

    [Header("UI")]
    [SerializeField] private Slider hPBar;
    [SerializeField] private Slider mPBar;
    public Transform interactionUIPos;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        SaveData data = SaveSystem.LoadPlayer();
        level = data.level;
        curHP = data.hp;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        this.transform.position = position;
        myAgent.SetDestination(this.transform.position);


        maxHP = charStat.orgData.HP[level - 1];
        curMP = charStat.orgData.HP[level - 1];
        maxMP = charStat.orgData.HP[level - 1];
        curExp = charStat.orgData.EXP[level - 1];
    }


    protected override void Start()
    {
        base.Start();
        canRun = true; //������ �� �ٷ� �� �� �ֵ���
        level = 1;
        curHP = charStat.orgData.HP[level - 1];
        maxHP = charStat.orgData.HP[level - 1];
        curMP = charStat.orgData.HP[level - 1];
        maxMP = charStat.orgData.HP[level - 1];
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
        HandleHP(dmg, 0f);
        StartCoroutine(Stunned(0.7f));
        curAnim[0].SetTrigger("IsHit");
        if (handleSlider == null) { handleSlider = StartCoroutine(SliderValue()); }
    }

    public void GetItemValue(int i, int value)
    {
        if (i == 1) {  HandleHP(0f, value); } // 1��Ÿ��, hpȸ��
        if (i == 2) { HandleMP(0f, value); } // 2��Ÿ��, mpȸ��
        if (handleSlider == null) { print("�ڷ�ƾ"); handleSlider = StartCoroutine(SliderValue()); }
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
        curHP = charStat.orgData.HP[level - 1];
        maxHP = charStat.orgData.HP[level - 1];
        curMP = charStat.orgData.HP[level - 1];
        maxMP = charStat.orgData.HP[level - 1];
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

    public void HandleHP(float consume, float increase)
    {
        curHP -= consume; // ����
        curHP += increase; // ����
    }
    public void HandleMP(float consume, float increase)
    {
        curMP -= consume; // ����
        curMP += increase; // ����
    }

    public IEnumerator SliderValue()
    {
        while (!isDead)
        {
            curHP = Mathf.Clamp(curHP, 0.1f, maxHP); //Ŭ������ �� ������Ʈ���� �۵��ؾ� �ϳ���?
            curMP = Mathf.Clamp(curMP, 0.1f, maxMP);
            curHP += 1 * Time.deltaTime; // �ð��� ȸ��
            curMP += 1 * Time.deltaTime; // �ð��� ȸ��
            hPBar.value = Mathf.Lerp(hPBar.value, curHP / maxHP * 100f, 0.2f);
            mPBar.value = Mathf.Lerp(mPBar.value, curMP / maxMP * 100f, 0.2f);
            if (curHP >= maxHP && curMP >= maxMP) { print("fef"); handleSlider = null; yield break; }
            yield return null;
        }
        handleSlider = null;
    }
}

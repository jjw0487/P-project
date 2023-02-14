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
    private int level; // 데이터 저장할 때 setter를 private로 만들어주자
    public int Level { get { return level; } } // 게터 <- 참조 대상
    [SerializeField]
    private float curHP; // 세터
    public float CurHP { get { return curHP; } } // 게터 <- 참조 대상
    [SerializeField] private float maxHP;
    [SerializeField]
    private float curMP; // 세터
    public float CurMP { get { return curMP; } } // 게터, 스킬셋에서 참조
    [SerializeField] private float maxMP;

    public Coroutine handleSlider; // skill에서 참조

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
        canRun = true; //시작할 때 바로 뛸 수 있도록
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
        if (i == 1) {  HandleHP(0f, value); } // 1번타입, hp회복
        if (i == 2) { HandleMP(0f, value); } // 2번타입, mp회복
        if (handleSlider == null) { print("코루틴"); handleSlider = StartCoroutine(SliderValue()); }
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
        curHP -= consume; // 감소
        curHP += increase; // 증가
    }
    public void HandleMP(float consume, float increase)
    {
        curMP -= consume; // 감소
        curMP += increase; // 증가
    }

    public IEnumerator SliderValue()
    {
        while (!isDead)
        {
            curHP = Mathf.Clamp(curHP, 0.1f, maxHP); //클램프는 꼭 업데이트에서 작동해야 하나요?
            curMP = Mathf.Clamp(curMP, 0.1f, maxMP);
            curHP += 1 * Time.deltaTime; // 시간당 회복
            curMP += 1 * Time.deltaTime; // 시간당 회복
            hPBar.value = Mathf.Lerp(hPBar.value, curHP / maxHP * 100f, 0.2f);
            mPBar.value = Mathf.Lerp(mPBar.value, curMP / maxMP * 100f, 0.2f);
            if (curHP >= maxHP && curMP >= maxMP) { print("fef"); handleSlider = null; yield break; }
            yield return null;
        }
        handleSlider = null;
    }
}

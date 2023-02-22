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
using static UnityEditor.Progress;

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
    private TMPro.TMP_Text[] playerStatus;
    private TMPro.TMP_Text[] playerAddedStatus;

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

    [SerializeField]
    private float sp;
    public float SP { get { return sp; } } // 스킬셋에서 데미지 계산 시 사용
    private float addedSP;
    

    public Coroutine handleSlider; // skill에서 참조

    [Header("UI")]
    [SerializeField] private Slider hPBar;
    [SerializeField] private Slider mPBar;
    public Transform interactionUIPos;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, SceneData.Inst.Inven, SceneData.Inst.interactableUIManager, SceneData.Inst.questManager, SceneData.Inst.interactableUIManager.skillBookData.tabs);
    }

    public void LoadPlayer()
    {
        SaveData data = SaveSystem.LoadPlayer();

        #region PLAYER DATA
        level = data.level;
        curHP = data.hp;
        Vector3 position;
        position.x = data.position[0]; 
        position.y = data.position[1];
        position.z = data.position[2];
        this.transform.position = position;
        myAgent.SetDestination(this.transform.position);
        SetStatus();
        SetStatusWindow();
        #endregion

        #region QUEST DATA
        for (int i = 0; i < data.questIndex.Length; i++)
        {
            SceneData.Inst.questManager.QM_LoadSavedQuest(data.questIndex[i]);
        }

        for (int i = 0; i < data.npcProgress.Length; i++)
        {
            SceneData.Inst.questManager.QM_LoadSavedNpcProgress(data.npcProgress[i]);
        }
        #endregion

        #region INVENTORY DATA

        SceneData.Inst.Inven.ClearSlots();

        for (int i = 0; i < data.itemCount.Length; i++)
        {
            SceneData.Inst.Inven.LoadItemData(data.items[i], data.itemCount[i]);
        }

        for(int i = 0; i<data.equipmentItems.Length; i++)
        {
            SceneData.Inst.Inven.LoadEquipmentItemData(data.equipmentItems[i]);
        }


        #endregion

        #region SKILL DATA
        for (int i = 0; i < SceneData.Inst.interactableUIManager.skillBookData.tabs.Length; i++)
        {
            SceneData.Inst.interactableUIManager.skillBookData.tabs[i].LoadSkillLevel(data.skillLevel[i]);
        }
        #endregion

        #region GOLD
        SceneData.Inst.interactableUIManager.LoadGold(data.gold);
        #endregion 
        

    }

    protected override void Start()
    {
        base.Start();
        canRun = true; //시작할 때 바로 뛸 수 있도록
        level = 1;
        addedSP = 0;
        playerStatus = SceneData.Inst.Inven.playerStatus;
        playerAddedStatus = SceneData.Inst.Inven.playerAddedStat;
        SetStatus();
        SetStatusWindow();
    }
    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.P))
        {
            GetEXP(20);
        }
    }

    public void SetStatusWindow()
    {
        // 0.레벨 1.필요경험치 2.마법공격력 3.방어력 4.최대체력 5.최대마력

        playerStatus[0].text = level.ToString();
        playerStatus[1].text = curExp.ToString();
        playerStatus[2].text = sp.ToString();
        //방어력 아직 없음 playerStatus[3].text = 
        playerStatus[4].text = charStat.orgData.HP[level - 1].ToString();
        playerStatus[5].text = charStat.orgData.MP[level - 1].ToString();

        //playerAddedStatus
        playerAddedStatus[2].text = "+" + addedSP.ToString();
        //방어력 아직 없음 playerAddedStatus[3].text = 
        //playerAddedStatus[4].text = charStat.orgData.HP[level - 1].ToString();
        //playerAddedStatus[5].text = charStat.orgData.MP[level - 1].ToString();
    }
    private void SetStatus()
    {
        sp = charStat.orgData.SP[level - 1] + addedSP;
        curHP = charStat.orgData.HP[level - 1];
        maxHP = charStat.orgData.HP[level - 1];
        curMP = charStat.orgData.MP[level - 1];
        maxMP = charStat.orgData.MP[level - 1];
        curExp = charStat.orgData.EXP[level - 1];
    }

    public void OnDmg(float dmg)
    {
        HandleHP(dmg, 0f);
        StartCoroutine(Stunned(0.7f));
        curAnim[2].SetTrigger("GetHit");
        curAnim[0].SetTrigger("IsHit");

        if (handleSlider == null) { handleSlider = StartCoroutine(SliderValue()); }
    }


    public void GetEquipedItemValue(int type, float equiped, float takeOff)
    {
        if (type == 1)
        {
            addedSP += equiped;
            addedSP -= takeOff;
            playerAddedStatus[2].text = "+" + addedSP.ToString(); //추가공격력
            sp = charStat.orgData.SP[level - 1] + addedSP; // sp 수치 업데이트0
            playerStatus[2].text = sp.ToString(); // 합산 공격력 출력
            return;
        }
        
    }

    public void GetItemValue(int i, int value)
    {
        if (handleSlider == null) { handleSlider = StartCoroutine(SliderValue()); }
        if (i == 1) {  HandleHP(0f, value); } // 1번타입, hp회복
        if (i == 2) { HandleMP(0f, value); } // 2번타입, mp회복
    }

    public void GetEXP(int exp)
    {
        curExp -= exp;
        playerStatus[1].text = curExp.ToString();
        if (curExp <= 0)
        {
            curExp *= -1; //음수를 양수로
            //curExp *= -1;
            LevelUp(+curExp);
        }
    }

    

    public void LevelUp(int rest)
    {
        ++level; // 일단 스크립터블에 영향이 안가도록 데이터는 건드리지 말고 이렇게 두자
        SceneData.Inst.interactableUIManager.SkillBook.GetComponent<SkillBook>().GetSkillPoint(level);
        SetStatus();
        curExp -= rest;
        if (curExp < 0)
        {
            curExp *= -1;
            LevelUp(+curExp);
        }
        if (SceneData.Inst.questManager != null) SceneData.Inst.questManager.QM_GetPlayerLevel(level);
        SetStatusWindow(); // 스텟창에 업데이트

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
            if (Mathf.Approximately(mPBar.value, maxMP) && Mathf.Approximately(hPBar.value, maxHP))
                { handleSlider = null; yield break; } 
            // 체력,마력에 변동 없을 때 더이상 돌지 않도록
            yield return null;
        }

        handleSlider = null;
    }
}

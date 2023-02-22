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

    [SerializeField]
    private float sp;
    public float SP { get { return sp; } } // ��ų�¿��� ������ ��� �� ���
    private float addedSP;
    

    public Coroutine handleSlider; // skill���� ����

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
        canRun = true; //������ �� �ٷ� �� �� �ֵ���
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
        // 0.���� 1.�ʿ����ġ 2.�������ݷ� 3.���� 4.�ִ�ü�� 5.�ִ븶��

        playerStatus[0].text = level.ToString();
        playerStatus[1].text = curExp.ToString();
        playerStatus[2].text = sp.ToString();
        //���� ���� ���� playerStatus[3].text = 
        playerStatus[4].text = charStat.orgData.HP[level - 1].ToString();
        playerStatus[5].text = charStat.orgData.MP[level - 1].ToString();

        //playerAddedStatus
        playerAddedStatus[2].text = "+" + addedSP.ToString();
        //���� ���� ���� playerAddedStatus[3].text = 
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
            playerAddedStatus[2].text = "+" + addedSP.ToString(); //�߰����ݷ�
            sp = charStat.orgData.SP[level - 1] + addedSP; // sp ��ġ ������Ʈ0
            playerStatus[2].text = sp.ToString(); // �ջ� ���ݷ� ���
            return;
        }
        
    }

    public void GetItemValue(int i, int value)
    {
        if (handleSlider == null) { handleSlider = StartCoroutine(SliderValue()); }
        if (i == 1) {  HandleHP(0f, value); } // 1��Ÿ��, hpȸ��
        if (i == 2) { HandleMP(0f, value); } // 2��Ÿ��, mpȸ��
    }

    public void GetEXP(int exp)
    {
        curExp -= exp;
        playerStatus[1].text = curExp.ToString();
        if (curExp <= 0)
        {
            curExp *= -1; //������ �����
            //curExp *= -1;
            LevelUp(+curExp);
        }
    }

    

    public void LevelUp(int rest)
    {
        ++level; // �ϴ� ��ũ���ͺ� ������ �Ȱ����� �����ʹ� �ǵ帮�� ���� �̷��� ����
        SceneData.Inst.interactableUIManager.SkillBook.GetComponent<SkillBook>().GetSkillPoint(level);
        SetStatus();
        curExp -= rest;
        if (curExp < 0)
        {
            curExp *= -1;
            LevelUp(+curExp);
        }
        if (SceneData.Inst.questManager != null) SceneData.Inst.questManager.QM_GetPlayerLevel(level);
        SetStatusWindow(); // ����â�� ������Ʈ

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
            if (Mathf.Approximately(mPBar.value, maxMP) && Mathf.Approximately(hPBar.value, maxHP))
                { handleSlider = null; yield break; } 
            // ü��,���¿� ���� ���� �� ���̻� ���� �ʵ���
            yield return null;
        }

        handleSlider = null;
    }
}

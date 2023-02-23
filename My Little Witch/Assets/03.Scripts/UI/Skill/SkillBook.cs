using System.Collections.Generic;
using UnityEngine;

public class SkillBook : PointerCheck
{
    public TMPro.TMP_Text point;

    public GameObject[] skillShutter;
    public SkillTab[] tabs;

    public int skillPoint;
    private int playerLevel;
    public List<SkillTab> tabList = new List<SkillTab>();

    void Awake()
    {
        skillPoint = 1;
        point.text = skillPoint.ToString();
    }

    private void Start()
    {
        tabList.Add(tabs[0]);
        tabList.Add(tabs[1]);
        tabList.Add(tabs[2]);
    }

    public void CalculateSkillPoint(int num)
    {
        skillPoint += num;
        point.text = skillPoint.ToString();
    }

    public void GetSkillPoint(int lv)
    {
        skillPoint += 1;
        point.text = skillPoint.ToString();
        playerLevel = lv;
        if (playerLevel == 5) { skillShutter[0].SetActive(false); tabList.Add(tabs[3]); }
        if (playerLevel == 7) { skillShutter[1].SetActive(false); tabList.Add(tabs[4]); }
        if (playerLevel == 10) { skillShutter[2].SetActive(false); tabList.Add(tabs[5]); }
        if (playerLevel == 12) { skillShutter[3].SetActive(false); tabList.Add(tabs[6]); }
        if (playerLevel == 15) { skillShutter[4].SetActive(false); tabList.Add(tabs[7]); }

        for (int i = 0; i < tabList.Count; ++i)
        {
            tabList[i].GetRestOfSkillPoint();
        }
    }



}

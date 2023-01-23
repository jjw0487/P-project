using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook : MonoBehaviour
{
    public TMPro.TMP_Text point;

    public int skillPoint;

    void Start()
    {
        skillPoint = 5;
        point.text = skillPoint.ToString();
    }

    public void CalculateSkillPoint(int num)
    {
        skillPoint += num;
        point.text = skillPoint.ToString();
    }

}

using UnityEngine;

public class SkillBook : PointerCheck
{
    public TMPro.TMP_Text point;

    public int skillPoint;

    void Awake()
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

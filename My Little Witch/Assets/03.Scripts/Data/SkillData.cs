using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = 3)]

public class SkillData : ScriptableObject
{
    public GameObject Effect;
    public float overlapRadius;
    public float remainTime;
    public float dmg;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = 3)]

public class SkillData : ScriptableObject
{
    public enum SkillType { Buff, Attck, Debuff, AttackNDebuff }
    public SkillType Type;


    public Vector3 performPos;
    public GameObject Effect;
    public float overlapRadius;
    public float remainTime;
    public float dmg;
    public float consumeMP;
    public string triggerName;
}

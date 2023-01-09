using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = 3)]

public class SkillData : ScriptableObject
{
    
    public enum SkillType { Buff, Attck, Debuff, AttackNDebuff }
    public enum ActionType { WaitBeforeAction, None }

    [Header("Type")]
    public SkillType Type;
    public ActionType Action;

    [Header("Mutual Information")]
    public Sprite sprite;
    public Vector3 performPos;
    public GameObject Effect;
    public float remainTime; // �ִϸ��̼� ���۵��� �������� ���� ����
    public float coolTime; // ��ų ���� ��Ÿ���� ����
    public float consumeMP;
    public string triggerName;

    [Header("Selective")]
    public float dmg;
    public float rangeOfSkill;
    public float overlapRadius;
    [Header("Selective::Debuff")]
    public float percentage;
    public float debuffTime;

}

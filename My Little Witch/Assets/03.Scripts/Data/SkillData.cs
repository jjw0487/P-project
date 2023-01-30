using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = 3)]

public class SkillData : ScriptableObject
{

    public enum SkillType { Buff, Attck, Debuff, AttackNDebuff, NormalAttack }
    public enum ActionType { WaitBeforeAction, None }

    [Header("Type")]
    public SkillType Type;
    public ActionType Action;

    [Header("Mutual Information")]
    public int level;
    public Sprite sprite;
    public Vector3 performPos;
    public GameObject Effect;
    public float remainTime; // 애니메이션 동작동안 움직임을 막기 위함
    public float[] coolTime; // 스킬 재사용 쿨타임을 위함
    public float consumeMP;
    public string triggerName;

    [Header("Selective")]
    public float[] dmg;
    public float rangeOfSkill;
    public float overlapRadius;

    [Header("Selective::Debuff")]
    public float[] percentage;
    public float[] debuffTime;

}

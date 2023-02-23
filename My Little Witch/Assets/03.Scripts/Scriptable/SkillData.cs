using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = 3)]

public class SkillData : ScriptableObject
{
    public enum SkillType { Buff, Attck, Debuff, AttackNDebuff, NormalAttack }
    public enum ActionType { WaitBeforeAction, None }
    public enum Orientation { immediate, Remain }

    [Header("Type")]
    public SkillType Type;
    public ActionType Action;
    public Orientation orientation;

    [Header("Mutual Information")]
    [SerializeField]
    private int Level;
    public int level { get { return Level; } set => Level = value; } // ���� setter�� ���� ����� ������

    [SerializeField]
    private float RemainTime;
    public float remainTime { get { return RemainTime; } } // �ִϸ��̼� ���۵��� �������� ���� ����

    [SerializeField]
    private float[] CoolTime;
    public float[] coolTime { get { return CoolTime; } }// ��ų ���� ��Ÿ���� ����

    [SerializeField]
    private float ConsumeMP;
    public float consumeMP { get { return ConsumeMP; } }

    [SerializeField]
    public string triggerName;
    public Sprite sprite;
    public Vector3 performPos;
    public GameObject Effect;

    [Header("Selective")]
    [SerializeField]
    private float[] Dmg;
    public float[] dmg { get { return Dmg; } }

    [SerializeField]
    private float RangeOfSkill; // ��ų ��밡�� �����Ÿ�
    public float rangeOfSkill { get { return RangeOfSkill; } }

    [SerializeField]
    private float OverlapRadius; // ��ų�� ���������Ǿ� ũ��
    public float overlapRadius { get { return OverlapRadius; } }

    [Header("Selective::Debuff")]
    [SerializeField]
    private float[] Percentage;
    public float[] percentage { get { return Percentage; } }

    [SerializeField]
    private float[] DebuffTime;
    public float[] debuffTime { get { return DebuffTime; } }

    [SerializeField]
    private int[] Upgrade;
    public int[] upgrade { get { return Upgrade; } }

}

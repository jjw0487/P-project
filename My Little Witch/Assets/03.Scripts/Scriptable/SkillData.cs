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
    public int level { get { return Level; } set => Level = value; } // 레벨 setter를 막는 방법을 연구해

    [SerializeField]
    private float RemainTime;
    public float remainTime { get { return RemainTime; } } // 애니메이션 동작동안 움직임을 막기 위함

    [SerializeField]
    private float[] CoolTime;
    public float[] coolTime { get { return CoolTime; } }// 스킬 재사용 쿨타임을 위함

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
    private float RangeOfSkill; // 스킬 사용가능 사정거리
    public float rangeOfSkill { get { return RangeOfSkill; } }

    [SerializeField]
    private float OverlapRadius; // 스킬의 오버랩스피어 크기
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

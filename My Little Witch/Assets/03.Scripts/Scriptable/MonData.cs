using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsData", menuName = "ScriptableObject/MonData", order = 1)]

public class MonData : ScriptableObject
{

    [SerializeField]
    private string MonsterName;
    public string monsterName { get { return MonsterName; } }
    [field: SerializeField] public float HP { get; private set; }

    [SerializeField]
    private float ATK;
    public float atk { get { return ATK; } }

    [SerializeField]
    private float DP;
    public float dp { get { return DP; } }

    [SerializeField]
    private float StrikingDist;
    public float strikingDist { get { return StrikingDist; } }

    [SerializeField]
    private float AgentSpeed;
    public float agentSpeed { get { return AgentSpeed; } }

    [SerializeField]
    private float AgentStopDist;

    public float agentStopDist { get { return AgentStopDist; } }

    [SerializeField]
    private float AttackRadius;
    public float attackRadius { get { return AttackRadius; } }

    [SerializeField]
    private float AttackSpeed;
    public float attackSpeed { get { return AttackSpeed; } }

    [SerializeField]
    private int EXP;
    public int exp { get { return EXP; } }

    [SerializeField]
    private ItemData[] DropItems;
    public ItemData[] dropItems { get { return DropItems; } }

    [SerializeField]
    private int Currency;
    public int currency { get { return Currency; } }



    public Vector3 HPlocalScale;
}

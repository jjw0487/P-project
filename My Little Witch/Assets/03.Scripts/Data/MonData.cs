using UnityEngine;

[CreateAssetMenu(fileName = "MonsData", menuName = "ScriptableObject/MonData", order = 1)]

public class MonData : ScriptableObject
{
    [field: SerializeField] public float HP { get; private set; }
    public float AT;
    public float DP;
    public float strikingDist;
    public float agentSpeed;
    public float agentStopDist;
    public float attackRadius;
    public float attackSpeed;
    public Vector3 HPlocalScale;
    public int EXP;

    public ItemData[] DropItems;
}

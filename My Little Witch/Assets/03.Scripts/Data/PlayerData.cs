using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public float HP;
    public float MP;
    public float AT;
    public int Level;
    public int[] EXP; // 레벨 당 필요 경험치
}

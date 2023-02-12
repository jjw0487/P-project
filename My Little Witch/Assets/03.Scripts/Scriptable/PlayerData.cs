using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public float[] HP;
    public float[] MP;
    public float[] SP; //마력
    public int[] EXP; // 레벨 당 필요 경험치
}

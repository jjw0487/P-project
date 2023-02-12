using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public float[] HP;
    public float[] MP;
    public float[] SP; //����
    public int[] EXP; // ���� �� �ʿ� ����ġ
}

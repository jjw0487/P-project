using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private float[] hp;
    public float[] HP { get { return hp; } }

    [SerializeField]
    private float[] mp;
    public float[] MP { get { return mp; } }

    [SerializeField]
    private float[] sp;
    public float[] SP { get { return sp; } } //����

    [SerializeField]
    private float[] dp; //����
    public float[] DP {get { return dp; } }

    [SerializeField]
    private int[] exp;
    public int[] EXP { get { return exp; } } // ���� �� �ʿ� ����ġ
}

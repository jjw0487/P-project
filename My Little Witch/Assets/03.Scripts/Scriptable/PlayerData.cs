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
    public float[] SP { get { return sp; } } //마력

    [SerializeField]
    private float[] dp; //방어력
    public float[] DP {get { return dp; } }

    [SerializeField]
    private int[] exp;
    public int[] EXP { get { return exp; } } // 레벨 당 필요 경험치
}

using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    Monster monster;
    Movement player;

    private void Awake()
    {
        if(GetComponent<Monster>() != null)
        {
            monster = GetComponent<Monster>();
        }
        if (GetComponentInParent<Movement>() != null)
        {
            player = GetComponentInParent<Movement>();
        }
    }

    public void OnMonAttack()
    {
        monster.MonAttack();
    }

    public void OnPlayerNormAtk()
    {
        player.C_OnNormAtk();
    }
}

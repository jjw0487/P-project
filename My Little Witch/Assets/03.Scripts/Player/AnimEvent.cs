using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    Monster monster;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }

    public void OnMonAttack()
    {
        monster.MonAttack();
    }
}

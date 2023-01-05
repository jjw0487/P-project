using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    Monster monster;
    Movement player;
    public SkillSet[] skillSet = null;

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
    /////////////MONSTER////////////////////
    
    public void OnMonAttack()
    {
        monster.MonAttack();
    }


    /////////////PLAYER//////////////////////
    
    public void OnPlayerNormAtk()
    {
        player.C_OnNormAtk();
    }

    public void OnPlayerANDSkill()
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].AND();
        //skillSet[i].AND();
    }

    public void OnPlayerBuffSkill()
    {

    }

    public void OnPlayerAtkSkill()
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].SkillAttack();
    }
}

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
    
    public void OnPlayerNormAtk() // 일반공격
    {
        player.C_OnNormAtk();
    }

    public void OnPlayerANDSkill() // 어택 n 디버프 스킬
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].AND();
        //skillSet[i].AND();
    }

    public void OnPlayerBuffSkill()
    {

    }

    public void OnPlayerAtkSkill() // 대기모션 있는 스킬
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].SkillAttack();
    }
    public void OnPlayerAtkWithoutWaitMotion() // 대기모션 없는 스킬
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].SkillAttackWithoutWaitMotion();
    }
}

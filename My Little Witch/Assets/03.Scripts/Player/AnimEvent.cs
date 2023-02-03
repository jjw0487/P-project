using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    Monster monster;
    Movement player;
    Demon demon;
    public SkillSet[] skillSet = null;

    private void Awake()
    {
        if (GetComponent<Monster>() != null)
        {
            monster = GetComponent<Monster>();
        }

        if (GetComponentInParent<Movement>() != null)
        {
            player = GetComponentInParent<Movement>();
        }

        if (GetComponentInParent<Demon>() != null)
        {
            demon = GetComponentInParent<Demon>();
        }

    }

    /////////////MONSTER////////////////////
    public void OnMonAttack()
    {
        monster.MonAttack();
    }


    ///////////// BOSS ////////////////////
    public void OnBossAttack1()
    {
        demon.DemonAtk1();
    }
    public void OnBossAttack2()
    {
        demon.DemonAtk2();
    }
    public void OnBossAttack3()
    {
       
    }

    /////////////PLAYER//////////////////////
    public void OnPlayerNormAtk() // �Ϲݰ���
    {
        player.C_OnNormAtk();
    }

    public void OnPlayerANDSkill() // ���� n ����� ��ų
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].AND();
        //skillSet[i].AND();
    }

    public void OnPlayerBuffSkill()
    {

    }

    public void OnPlayerAtkSkill() // ����� �ִ� ��ų
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].SkillAttack();
    }
    public void OnPlayerAtkWithoutWaitMotion() // ����� ���� ��ų
    {
        skillSet[GetComponent<Animator>().GetInteger("SkillNum")].SkillAttackWithoutWaitMotion();
    }
}

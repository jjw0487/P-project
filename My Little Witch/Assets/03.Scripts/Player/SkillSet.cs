using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct SkillStat
{
    public SkillData orgData;
}

public class SkillSet : MonoBehaviour
{
    [Header("Information")]
    public SkillStat skillStat;
    public Image[] img;

    [Header("Object")]
    public Skill fromSkill;
    public Transform myCharacter;

    private Camera mainCamera;
    private Vector3 SkillHitPoint;
    private bool inCoolTime;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        img[0].sprite = skillStat.orgData.sprite; // ���߿� ����â ����� ������Ʈ�� �־���� �� 
    }

    public void PerformSkill()
    {
        if(!inCoolTime && fromSkill.myMagicCircuit.value > skillStat.orgData.consumeMP && !fromSkill.myPlayer.stun)
        {
            if(skillStat.orgData.Type == SkillData.SkillType.Buff) // ����
            {
                if(skillStat.orgData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    Buff();
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                }
            }
            else if(skillStat.orgData.Type == SkillData.SkillType.Attck) // ����
            {
                if (skillStat.orgData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    //SkillAttackWithoutWaitMotion(); // �ִ��̺�Ʈ���� ����
                    StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime)); 
                }
            }
            else if(skillStat.orgData.Type == SkillData.SkillType.Debuff) // �����
            {
                if (skillStat.orgData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                }
                
            }
            else //���� N �����
            {
                if (skillStat.orgData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform_AND(5f));
                }
                else
                {
                    AND(); // ���߿� ��ų ���� AnimEvent�� �ٲܰŸ� ������
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                }  
            }
        }
        else
        {
            fromSkill.myPlayer.state[2].text = "������ �����մϴ�.";
        }
        
    }

    public void Buff()
    {
        GameObject obj = Instantiate(skillStat.orgData.Effect, myCharacter.transform.position + skillStat.orgData.performPos, Quaternion.identity);
        fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
    }
    public void SkillAttackWithoutWaitMotion() // �Ϲ� ��ų���� <- �ִ��̺�Ʈ���� ����
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, myCharacter.transform.position + skillStat.orgData.performPos, Quaternion.identity);
        SkillOverlapColWithoutWaitMotion();
    }

    public void SkillAttack() // ���� with Waiting Motion <- �ִ��̺�Ʈ���� ����
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
        SkillOverlapCol(); 
    }

    public void AND() // ���� N ����� with Waiting Motion <- �ִ��̺�Ʈ���� ����
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
        SkillOverlapCol_AND();
    }

    public void SkillOverlapColWithoutWaitMotion() // ��� ��� ���� 
    {
        Collider[] hitColliders = Physics.OverlapSphere(myCharacter.transform.position, skillStat.orgData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                /*Monster mon = col.GetComponentInParent<Monster>();
                if (mon == null) continue;*/   //���߿� nullref ������ ����ó�� �������.
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(skillStat.orgData.dmg);
                }
            }
        }
    }

    public void SkillOverlapCol() 
    {
        Collider[] hitColliders = Physics.OverlapSphere(SkillHitPoint, skillStat.orgData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
          if(col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                /*Monster mon = col.GetComponentInParent<Monster>();
                if (mon == null) continue;*/   //���߿� nullref ������ ����ó�� �������.
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(skillStat.orgData.dmg);
                }
            }
        }
    }

    public void SkillOverlapCol_AND()
    {
        Collider[] hitColliders = Physics.OverlapSphere(SkillHitPoint, skillStat.orgData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                if (!col.GetComponentInParent<Monster>().isDead)
                {
                    col.GetComponentInParent<Monster>().OnDamage(skillStat.orgData.dmg);
                    col.GetComponentInParent<Monster>().OnDebuff(skillStat.orgData.debuffTime, skillStat.orgData.percentage);
                }
            }
        }
    }

    public IEnumerator WaitForThePerform(float cool)
    {
        fromSkill.canMove = false;
        fromSkill.canSkill = false;

        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // �����
        fromSkill.SkillLimit.SetActive(true); // ��ų �����Ÿ� ǥ�� 
        fromSkill.rangeOfSkills.localScale = new Vector3 (skillStat.orgData.rangeOfSkill, 0.01f, skillStat.orgData.rangeOfSkill); // ��ų �����Ÿ�
        
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // ��� ������
            {
                StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                yield break; //�ٷ� ����
            }

            if (Input.GetMouseButtonDown(0)) // ��� �ð� ���� ���콺 �Է�
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName); //�ִϸ��̼�
                    SkillHitPoint = skillStat.orgData.performPos + hitData.point; // ��ų������Ʈ ������ �����
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // ���� �������� �Ĵٺ���
                    //SkillAttack(); // �ִ��̺�Ʈ���� �۵�
                    fromSkill.SkillLimit.SetActive(false); // �����Ÿ� ����
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime)); //��Ÿ��
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư���� ���
            {
                //���
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true; 
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break; //��Ÿ�� ���� ����
            }
            
            yield return null;
        }
        fromSkill.SkillLimit.SetActive(false);
        fromSkill.canMove = true; //�ð��� ������
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }


    public IEnumerator WaitForThePerform_AND(float cool) // ���ݰ� �����
    {
        fromSkill.canMove = false;
        fromSkill.canSkill = false;
        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // �����
        fromSkill.SkillLimit.SetActive(true); // ��ų �����Ÿ�    
        fromSkill.rangeOfSkills.localScale = new Vector3(skillStat.orgData.rangeOfSkill, 0.01f, skillStat.orgData.rangeOfSkill); // ��ų �����Ÿ�

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // ��� ������
            {
                StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                yield break; // �ٷ� ����
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName); // �ִϸ��̼�
                    SkillHitPoint = skillStat.orgData.performPos + hitData.point; // ��ų�� ���� ������ ���� �޾Ƶΰ�
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // ��ų �� �������� �Ĵٺ���
                    //AND(); //�ִ��̺�Ʈ���� �۵�
                    fromSkill.SkillLimit.SetActive(false); //�����Ÿ�
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime)); //��Ÿ��
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //���
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break;
            }
            yield return null;
        }
        fromSkill.SkillLimit.SetActive(false);
        fromSkill.canMove = true; //�ð��� ������
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }

    IEnumerator CoolTime(float cool)
    {
        float coolTime = cool;
        inCoolTime = true; //Ʈ�縦 �ְ�
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            img[1].fillAmount = 1f * (cool / coolTime);
            yield return null;
        }
        inCoolTime = false; //�ð��� ������
    }
}

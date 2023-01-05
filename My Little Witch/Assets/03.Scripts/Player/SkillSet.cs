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
    public Image img;

    [Header("Object")]
    public Skill fromSkill;
    public Transform myCharacter;
    Camera mainCamera;
    Vector3 SkillHitPoint;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        img.sprite = skillStat.orgData.sprite; // ������Ʈ �������
    }

    public void PerformSkill()
    {
        if(fromSkill.myMagicCircuit.value > skillStat.orgData.consumeMP)
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
                    SkillAttack();
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
                    
                }
                /*GameObject obj = Instantiate(skillStat.orgData.Effect, this.transform.position + Vector3.up, Quaternion.identity);
                fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
                fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
                StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime, obj));
                SkillOverlapCol(); // �����*/
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
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
    }

    public void SkillAttack()
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
        SkillOverlapCol(); // ����
    }

    public void AND() // ���� N �����
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
        SkillOverlapCol_AND(); 
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
        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos");
        fromSkill.SkillLimit.SetActive(true);
        fromSkill.rangeOfSkills.localScale = new Vector3 (skillStat.orgData.rangeOfSkill, 0.01f, skillStat.orgData.rangeOfSkill);
        
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // ��� ������
            {
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                yield break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
                    SkillHitPoint = skillStat.orgData.performPos + hitData.point;
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized);
                    //SkillAttack();
                    fromSkill.SkillLimit.SetActive(false);
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


    public IEnumerator WaitForThePerform_AND(float cool)
    {
        fromSkill.canMove = false;
        fromSkill.canSkill = false;
        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos");
        fromSkill.SkillLimit.SetActive(true);
        fromSkill.rangeOfSkills.localScale = new Vector3(skillStat.orgData.rangeOfSkill, 0.01f, skillStat.orgData.rangeOfSkill);

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // ��� ������
            {
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                yield break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
                    SkillHitPoint = skillStat.orgData.performPos + hitData.point;
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized);
                    //AND();
                    fromSkill.SkillLimit.SetActive(false);
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
}

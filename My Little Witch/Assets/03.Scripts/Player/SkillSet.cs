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
        img[0].sprite = skillStat.orgData.sprite; // 나중에 스탯창 만들고 업데이트에 넣어줘야 함 
    }

    public void PerformSkill()
    {
        if(!inCoolTime && fromSkill.myMagicCircuit.value > skillStat.orgData.consumeMP && !fromSkill.myPlayer.stun)
        {
            if(skillStat.orgData.Type == SkillData.SkillType.Buff) // 버프
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
            else if(skillStat.orgData.Type == SkillData.SkillType.Attck) // 어택
            {
                if (skillStat.orgData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform(5f));
                }
                else
                {
                    //SkillAttackWithoutWaitMotion(); // 애님이벤트에서 실행
                    StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime)); 
                }
            }
            else if(skillStat.orgData.Type == SkillData.SkillType.Debuff) // 디버프
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
            else //공격 N 디버프
            {
                if (skillStat.orgData.Action == SkillData.ActionType.WaitBeforeAction)
                {
                    StartCoroutine(WaitForThePerform_AND(5f));
                }
                else
                {
                    AND(); // 나중에 스킬 전부 AnimEvent로 바꿀거면 지우자
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                }  
            }
        }
        else
        {
            fromSkill.myPlayer.state[2].text = "마력이 부족합니다.";
        }
        
    }

    public void Buff()
    {
        GameObject obj = Instantiate(skillStat.orgData.Effect, myCharacter.transform.position + skillStat.orgData.performPos, Quaternion.identity);
        fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
    }
    public void SkillAttackWithoutWaitMotion() // 일반 스킬어택 <- 애님이벤트에서 실행
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, myCharacter.transform.position + skillStat.orgData.performPos, Quaternion.identity);
        SkillOverlapColWithoutWaitMotion();
    }

    public void SkillAttack() // 어택 with Waiting Motion <- 애님이벤트에서 실행
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
        SkillOverlapCol(); 
    }

    public void AND() // 어택 N 디버프 with Waiting Motion <- 애님이벤트에서 실행
    {
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime));
        SkillOverlapCol_AND();
    }

    public void SkillOverlapColWithoutWaitMotion() // 대기 모션 없는 
    {
        Collider[] hitColliders = Physics.OverlapSphere(myCharacter.transform.position, skillStat.orgData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                /*Monster mon = col.GetComponentInParent<Monster>();
                if (mon == null) continue;*/   //나중에 nullref 나오면 예외처리 해줘야함.
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
                if (mon == null) continue;*/   //나중에 nullref 나오면 예외처리 해줘야함.
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

        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // 대기모션
        fromSkill.SkillLimit.SetActive(true); // 스킬 사정거리 표시 
        fromSkill.rangeOfSkills.localScale = new Vector3 (skillStat.orgData.rangeOfSkill, 0.01f, skillStat.orgData.rangeOfSkill); // 스킬 사정거리
        
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // 얻어 맞으면
            {
                StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                yield break; //바로 종료
            }

            if (Input.GetMouseButtonDown(0)) // 대기 시간 내에 마우스 입력
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName); //애니메이션
                    SkillHitPoint = skillStat.orgData.performPos + hitData.point; // 스킬힛포인트 변수를 만들고
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // 시전 방향으로 쳐다보고
                    //SkillAttack(); // 애님이벤트에서 작동
                    fromSkill.SkillLimit.SetActive(false); // 사정거리 끄고
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime)); //쿨타임
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼으로 취소
            {
                //취소
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true; 
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break; //쿨타임 없이 종료
            }
            
            yield return null;
        }
        fromSkill.SkillLimit.SetActive(false);
        fromSkill.canMove = true; //시간이 끝나면
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }


    public IEnumerator WaitForThePerform_AND(float cool) // 공격과 디버프
    {
        fromSkill.canMove = false;
        fromSkill.canSkill = false;
        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos"); // 대기모션
        fromSkill.SkillLimit.SetActive(true); // 스킬 사정거리    
        fromSkill.rangeOfSkills.localScale = new Vector3(skillStat.orgData.rangeOfSkill, 0.01f, skillStat.orgData.rangeOfSkill); // 스킬 사정거리

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;

            if (fromSkill.myPlayer.stun) // 얻어 맞으면
            {
                StartCoroutine(CoolTime(skillStat.orgData.coolTime));
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                yield break; // 바로 종료
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("SkillLimit")))
                {
                    fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName); // 애니메이션
                    SkillHitPoint = skillStat.orgData.performPos + hitData.point; // 스킬이 나갈 포지션 값을 받아두고
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized); // 스킬 쏜 방향으로 쳐다보고
                    //AND(); //애님이벤트에서 작동
                    fromSkill.SkillLimit.SetActive(false); //사정거리
                    StartCoroutine(CoolTime(skillStat.orgData.coolTime)); //쿨타임
                    yield break;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //취소
                fromSkill.SkillLimit.SetActive(false);
                fromSkill.canMove = true;
                fromSkill.canSkill = true;
                fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
                yield break;
            }
            yield return null;
        }
        fromSkill.SkillLimit.SetActive(false);
        fromSkill.canMove = true; //시간이 끝나면
        fromSkill.canSkill = true;
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }

    IEnumerator CoolTime(float cool)
    {
        float coolTime = cool;
        inCoolTime = true; //트루를 주고
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            img[1].fillAmount = 1f * (cool / coolTime);
            yield return null;
        }
        inCoolTime = false; //시간이 끝나면
    }
}

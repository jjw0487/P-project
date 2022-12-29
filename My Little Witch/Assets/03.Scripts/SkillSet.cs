using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

[Serializable]
public struct SkillStat
{
    public SkillData orgData;
}

public class SkillSet : MonoBehaviour
{
    public SkillStat skillStat;
    public Skill fromSkill;

    public Transform myCharacter;

    public ScriptableObject mySkill;

    Camera mainCamera;
    Vector3 SkillHitPoint;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void PerformSkill()
    {
        if(fromSkill.myMagicCircuit.value > skillStat.orgData.consumeMP)
        {
            if(skillStat.orgData.Type == SkillData.SkillType.Buff)
            {
                Buff();
            }
            else if(skillStat.orgData.Type == SkillData.SkillType.Attck)
            {
                StartCoroutine(WaitForThePerform(5f));
            }
            else if(skillStat.orgData.Type == SkillData.SkillType.Debuff)
            {
                GameObject obj = Instantiate(skillStat.orgData.Effect, this.transform.position + Vector3.up, Quaternion.identity);
                fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
                fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
                StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime, obj));
                SkillOverlapCol(); // 디버프
            }
            else
            {
                GameObject obj = Instantiate(skillStat.orgData.Effect, this.transform.position + Vector3.up, Quaternion.identity);
                fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
                fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
                StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime, obj));
                SkillOverlapCol(); // 디버프 and 어택
            }
        }
        else
        {
            fromSkill.myPlayer.state[2].text = "마력이 부족합니다.";
        }
        
    }

    public void Buff()
    {
        GameObject obj = Instantiate(skillStat.orgData.Effect, fromSkill.myCharacter.transform.position + skillStat.orgData.performPos, Quaternion.identity);
        fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime, obj));
    }

    public void SkillAttack()
    {
        GameObject obj = Instantiate(skillStat.orgData.Effect, SkillHitPoint, Quaternion.identity);
        fromSkill.myPlayer.curAnim[0].SetTrigger(skillStat.orgData.triggerName);
        fromSkill.myMagicGage.HandleMP(skillStat.orgData.consumeMP);
        StartCoroutine(fromSkill.Chill(skillStat.orgData.remainTime, obj));
        //SkillOverlapCol(); // 어택
    }
    public void SkillOverlapCol()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, skillStat.orgData.overlapRadius);
        foreach (Collider col in hitColliders)
        {
          //////
        }
    }
    
    public IEnumerator WaitForThePerform(float cool)
    {

        fromSkill.myPlayer.curAnim[0].SetTrigger("WaitForPos");
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("Ground")))
                {
                    SkillHitPoint = skillStat.orgData.performPos + hitData.point;
                    myCharacter.transform.rotation = Quaternion.LookRotation((hitData.point - myCharacter.transform.position).normalized);
                    SkillAttack();
                    yield break;
                }
            }
            yield return null;
        }
        fromSkill.myPlayer.curAnim[0].SetTrigger("Idle");
    }
}

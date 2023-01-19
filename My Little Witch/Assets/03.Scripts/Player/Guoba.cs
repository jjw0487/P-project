using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static Monster;

public class Guoba : MonoBehaviour
{
    public enum GuobaMachineState { Create, Target, Attack, Delay, Destroy }
    public GuobaMachineState GuobaState = GuobaMachineState.Create;

    private Transform myTarget;

    public SkillData myData;
    //public GameObject prefab;

    void Start()
    {
        Gu_ChangeState(GuobaMachineState.Create); // 업데이트 없이 만들어보자
        Destroy(gameObject, myData.percentage);
    }

    public void Gu_ChangeState(GuobaMachineState what)
    {
        //if (GuobaMachineState == what) return;
        GuobaState = what;
        switch (GuobaState)
        {
            case GuobaMachineState.Create:
                Gu_ChangeState(GuobaMachineState.Target);
                break;
            case GuobaMachineState.Target:
                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 10f);
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Monster")) // 검출된 콜라이더 중에 몬스터가 있다면
                    {
                        print("확인");
                        if (!col.GetComponentInParent<Monster>().isDead) //죽지 않았다면
                        {
                            myTarget = col.transform; // 타겟을 그 콜라이더로
                            Gu_ChangeState(GuobaMachineState.Attack); // 어택으로 전환
                            break;
                        }
                    }
                    /*else //검출이 안됐다면
                    {
                        StartCoroutine(DelayTarget()); //딜레이를 주고 다시 타겟 실행
                        break;
                    }*/
                }
                break;
            case GuobaMachineState.Attack:
                StartCoroutine(Attacking());
                break;
        }
    }
    IEnumerator DelayTarget()
    {
        yield return new WaitForSeconds(2f);
        Gu_ChangeState(GuobaMachineState.Target);
    }
    IEnumerator Attacking()
    {
        Vector3 dir = (myTarget.transform.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rot, t);
            yield return null;
        }
        //Instantiate(prefab, this.transform.position + new Vector3(0,1,1), Quaternion.identity);
        StartCoroutine(DelayTarget()); // 다시 타겟을 실행하여 타겟을 새로 찾음
    }
}

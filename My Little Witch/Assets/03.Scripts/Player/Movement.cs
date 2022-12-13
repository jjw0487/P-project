using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private Rigidbody rigidbody;

    [Header("Movement")]
    public Animator curAnim;
    public float Speed = 4f;
    public float dashSpeed = 7f;
    public float rotSpeed = 10f;
    private Vector3 dir = Vector3.zero;
    private float totalDist;
    public bool run;
    public bool canRun = true;


    [Header("UI")]
    public TMPro.TMP_Text state;
    public StaminaBar myStamina;
    public Slider myStaminaSlider;
    public HPBar myHPBar;

    [Header("Skill")]
    public Skill mySkill;

    [Header("Game Setting")]
    public Transform AttackMark;

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        state.text = "Idle";
        canRun = true; //������ �� �ٷ� �� �� �ֵ���
    }

    void Update()
    {
        if(mySkill.canMove)
        {
            CharacterMovement(); //��ý���
            Running();
        }

    }
    public void OnDmg(float dmg)
    {
        myHPBar.HandleHP(dmg);
    }
    void Running()
    {
        /*if (run) // �޸���
        {
            rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
        }*/

        if (Mathf.Approximately(myStaminaSlider.value, 1f))
        //���¹̳� ���� ����� 0�� �ٻ�ġ�� ���� ��
        {
            canRun = false;
            if (totalDist > 0.0f)
            {
                curAnim.SetBool("IsWalking", true);
            }
            else
            {
                curAnim.SetBool("IsWalking", false);
            }

            curAnim.SetBool("IsRunning", false);

            run = false;
        }
        else
        {
            //canRun= true;
            if (Input.GetKey(KeyCode.LeftShift) && totalDist > 0.0f && canRun)
            // ����Ʈ�� ������, �̵��Ÿ��� ������ canRun �� false�� �ƴ� ��
            {
                run = true;
                curAnim.SetBool("IsRunning", true);
                rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
            }
            else // �̵��Ÿ����� 0���� ���� �� shift�� �޸��� �ߵ� ���� �� �ֵ���
            {
                run = false;
                curAnim.SetBool("IsRunning", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !canRun)
        // ����Ʈ Ű�� ������, canRun �� false�� ��
        {
            canRun = true;
        }
    }

    void StateNotice()
    {
        if (curAnim.GetBool("IsWalking"))
        {
            state.text = "Walk";
        }
        
        if (curAnim.GetBool("IsRunning"))
        {
            state.text = "Run";
        }
    }


    void CharacterMovement()
    {
       
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        totalDist = dir.magnitude;

        rigidbody.MovePosition(this.transform.position + dir * Speed * Time.deltaTime);

        if (totalDist > 0.0f)
        {
            curAnim.SetBool("IsWalking", true);
            StateNotice();
        }
        if (totalDist <= 0.0f)
        {
            curAnim.SetBool("IsWalking", false);
            state.text = "Idle";
        }

        

        if (dir != Vector3.zero) //������ ���ΰ� �ƴ϶�� Ű �Է��� ��
        {
            // ������ ���ư� �� + �������� ���ư��µ� �ݴ�������� �������� Ű�� ������ �� -�������� ȸ���ϸ鼭 ����� ������ �����ϱ����� (��ȣ�� ���� �ݴ��� ��츦 üũ�ؼ� ��¦�� �̸� �����ִ� �ڵ�) ��Ƴ׿�... 
            // ���� �ٶ󺸴� ������ ��ȣ != ���ư� ���� ��ȣ
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) ||
                Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                //�츮�� �̵��� �� x �� z �ۿ� ����� ���ϹǷ�
                transform.Rotate(0, 1, 0); // ��¦�� ȸ��
                //�� �ݴ������ ������ ȸ�����ϴ� ���� ����
                //�̸� ȸ���� ���� ���Ѽ� ���ݴ��� ��츦 ����
            }

            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
            // Slerp�� ���� Lerp�� ���� ���Ǹ� �غ��� �� �� ���ƿ� 
            // ĳ������ �չ����� dir Ű���带 ���� �������� ĳ���� ȸ��
            //Lerp�� ���� ���ϴ� ������� ������ ȸ��
        }
    }

    

}

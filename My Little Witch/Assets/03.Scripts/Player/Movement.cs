using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public TMPro.TMP_Text state;

    new Rigidbody rigidbody;
    public Animator curAnim;
    public Skill mySkill;

    public float Speed = 4f;
    public float dashSpeed = 7f;
    public float rotSpeed = 10f;



    private Vector3 dir= Vector3.zero;
    private float totalDist;
    public bool run;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        //state.text = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        if(mySkill.canMove)
        {
            CharacterMovement(); //��ý���

            if (Input.GetKey(KeyCode.LeftShift) && totalDist > 0.0f)
            {
                StateNotice();
                run = true;
                Running();
            }
            else
            {
                run = false;
                curAnim.SetBool("IsRunning", false);
            }
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

    void Running()
    {
        curAnim.SetBool("IsRunning", true);
        rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
        StateNotice();
    }
}

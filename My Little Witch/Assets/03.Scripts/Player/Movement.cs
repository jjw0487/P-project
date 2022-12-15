using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private Rigidbody rigidbody;

    [Header("Character")]
    public ONWHAT onWhat = ONWHAT.Street;
    public GameObject KK;
    public GameObject BR;

    [Header("Movement")]
    public Animator[] curAnim;
    public float Speed = 4f;
    public float dashSpeed = 7f;
    public float rotSpeed = 10f;
    private Vector3 dir = Vector3.zero;
    private float totalDist;
    public bool run;
    public bool canRun = true;
    public bool stun = false;

    [Header("OnTheBroom")]
    public float B_Speed = 10f;
    public float B_RotSpeed = 3f;
    private float B_totalDist;
    public float B_jumpHeight = 0.2f;


    [Header("UI")]
    public TMPro.TMP_Text state;
    public StaminaBar myStamina;
    public Slider myStaminaSlider;
    public HPBar myHPBar;

    [Header("Skill")]
    public Skill mySkill;

    [Header("Game Setting")]
    public Transform AttackMark;


    public enum ONWHAT { Street, Broom }

    public void ChangeState(ONWHAT what)
    {
        if (onWhat == what) return;
        onWhat = what;
        switch(onWhat)
        {
            case ONWHAT.Street:
                KK.SetActive(true);
                BR.SetActive(false);
                break;
            case ONWHAT.Broom:
                KK.SetActive(false);
                BR.SetActive(true);
                break;
        }
    }

    void Start()
    {
        ChangeState(ONWHAT.Street);
        rigidbody = this.GetComponent<Rigidbody>();
        state.text = "Idle";
        canRun = true; //������ �� �ٷ� �� �� �ֵ���
    }

    void Update()
    {
        if(ONWHAT.Street == onWhat)
        {
            if (mySkill.canMove && !stun)
            {
                C_Movement(); //��ý���
                Running();
            }
        }

        if(ONWHAT.Broom == onWhat)
        {
            B_Movement();
        }

        if(Input.GetKeyDown(KeyCode.L)&& onWhat == ONWHAT.Street)
        {
            ChangeState(ONWHAT.Broom);
        }

        if (Input.GetKeyDown(KeyCode.L) && onWhat == ONWHAT.Broom)
        {
            ChangeState(ONWHAT.Street);
        }

    }
    public void OnDmg(float dmg)
    {
        myHPBar.HandleHP(dmg);
        StartCoroutine(Stunned(0.7f));
        curAnim[0].SetTrigger("IsHit");
        //curAnim[1].SetTrigger("IsHit");
    }
    void Running()
    {
        if (Mathf.Approximately(myStaminaSlider.value, 0.0f))
        //���¹̳� ���� ����� 0�� �ٻ�ġ�� ���� ��
        {
            canRun = false;
            if (totalDist > 0.0f)
            {
                curAnim[0].SetBool("IsWalking", true);
            }
            else
            {
                curAnim[0].SetBool("IsWalking", false);
            }

            curAnim[0].SetBool("IsRunning", false);

            run = false;
        }
        else
        {
            //canRun= true;
            if (Input.GetKey(KeyCode.LeftShift) && totalDist > 0.0f && canRun)
            // ����Ʈ�� ������, �̵��Ÿ��� ������ canRun �� false�� �ƴ� ��
            {
                run = true;
                curAnim[0].SetBool("IsRunning", true);
                rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
            }
            else // �̵��Ÿ����� 0���� ���� �� shift�� �޸��� �ߵ� ���� �� �ֵ���
            {
                run = false;
                curAnim[0].SetBool("IsRunning", false);
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
        if (curAnim[0].GetBool("IsWalking"))
        {
            state.text = "Walk";
        }
        
        if (curAnim[0].GetBool("IsRunning"))
        {
            state.text = "Run";
        }
    }


    void C_Movement()
    {
       
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        totalDist = dir.magnitude;

        rigidbody.MovePosition(this.transform.position + dir * Speed * Time.deltaTime);

        if (totalDist > 0.0f)
        {
            curAnim[0].SetBool("IsWalking", true);
            StateNotice();
        }
        if (totalDist <= 0.0f)
        {
            curAnim[0].SetBool("IsWalking", false);
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
            // Lerp�� ���� ���ϴ� ������� ������ ȸ��
        }
    }
    void B_Movement()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        totalDist = dir.magnitude;

        GetComponent<Rigidbody>().MovePosition(this.transform.position + dir * B_Speed * Time.deltaTime);
        transform.forward = Vector3.Lerp(transform.forward, dir, B_RotSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 jumpPower = Vector3.up * B_jumpHeight;
            GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);
        }
    }

    IEnumerator Stunned(float cool) // �� �����̰� �ϴ� ��ų��
    {
        stun = true;
        while (cool > 0.0f)
        {
            state.text = "Stun";
            cool -= Time.deltaTime;
            yield return null;
        }
        stun = false;
    }



}

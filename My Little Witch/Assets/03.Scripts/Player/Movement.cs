using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Movement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Movement : MonoBehaviour
{
    public new Rigidbody rigidbody;

    [Header("Character")]
    public ONWHAT onWhat = ONWHAT.Street;
    public GameObject KK;
    public GameObject BR;

    [Header("Movement")]
    public Animator[] curAnim;
    public float Speed = 4f;
    public float dashSpeed = 7f;
    public float rotSpeed = 10f;
    public float JumpHeight = 3f;
    private Vector3 dir = Vector3.zero;
    public LayerMask layer;
    private float totalDist;
    public bool run;
    public bool canRun = true;
    public bool stun = false;
    public bool ground = true;


    [Header("BroomMovement")]
    public float B_Speed = 10f;
    public float B_RotSpeed = 3f;
    private float B_totalDist;
    public float B_AddFloatPower = 0.2f;
    public float restrictedHeight = 20f;


    [Header("UI")]
    public TMPro.TMP_Text[] state;
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
        //if (onWhat == what) return;
        onWhat = what;
        switch(onWhat)
        {
            case ONWHAT.Street:
                rigidbody.drag = 0f;
                KK.SetActive(true);
                BR.SetActive(false);
                break;
            case ONWHAT.Broom:
                rigidbody.drag = 6f;
                BR.SetActive(true);
                KK.SetActive(false);
                break;
        }
    }
    public void StateProcess()
    {
        switch (onWhat)
        {
            case ONWHAT.Street:
                if (mySkill.canMove && !stun)
                {
                    C_Movement(); //��ý���
                    Running();
                }
                break;
            case ONWHAT.Broom:
                B_Movement();
                break;
        }
    }

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        ChangeState(ONWHAT.Street);        
        state[0].text = "Idle";
        canRun = true; //������ �� �ٷ� �� �� �ֵ���
    }

    void Update()
    {
        
        StateProcess();
        CheckGround();

        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchingCharacter();
        }

        

    }

    public void CheckGround() // �������� ����, ������ ���� ���� ����
    {

        //�Ǻ� ��ġ�� �߳��̱� ������ ĳ���� ���� ���� �پ������ ������ �� ���� ������ (Vector3.up * 0.2f)�� ��¦ �÷��� ���̸� ��
        // Vector3.down �Ʒ��ϱ� �Ʒ��� ���� ��
        // �󸶸�ŭ�� �Ÿ��� �������� ����� = 0.4f
        // = �������� ��ǵ� ĳ������ �� ������ 0.2 ��ŭ ���� ��ġ���� �Ʒ��������� ����̰� 0.4 ��ŭ�� �������� �߻� �ɰ��̴�
        // �� ���� �ȿ��� �츮�� ������ ���̾ ������ �Ǹ� �� ������ out hit �� ��ƶ�

        // ���� ������Ʈ�� �ű�� �������� ���� ��ġ��(0.4f, 0.2f) �� �����ϰ� �ؾ��ϴ� ������ �� �ֳ׿�
        if(onWhat == ONWHAT.Street)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.localPosition + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer))
            {
                ground = true;
                state[1].text = "Ground";
            }
            else
            {
                ground = false;
                state[1].text = "InAir";
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.localPosition, Vector3.down, out hit, 0.3f, layer))
            {
                ground = true;
                state[1].text = "Ground";
            }
            else
            {
                ground = false;
                state[1].text = "InAir";
            }
        }
        
    }
    void SwitchingCharacter()
    {
        if(ONWHAT.Broom != onWhat)
        {
            onWhat = ONWHAT.Broom;
            ChangeState(ONWHAT.Broom);
        }
        else
        {
            onWhat = ONWHAT.Street;
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
            state[0].text = "Walk";
        }
        
        if (curAnim[0].GetBool("IsRunning"))
        {
            state[0].text = "Run";
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
            state[0].text = "Idle";
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            state[0].text = "Jump";
            curAnim[0].SetTrigger("Jump");
            StartCoroutine(Jumping(0.6f, 1.4f));
            
        }

    }
    void B_Movement()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        totalDist = dir.magnitude;

        rigidbody.MovePosition(this.transform.position + dir * B_Speed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, dir, B_RotSpeed * Time.deltaTime);

        if (this.transform.position.y < restrictedHeight) // ��������
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 jumpPower = Vector3.up * B_AddFloatPower;
                GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);
            }
        }
        
    }

    IEnumerator Stunned(float cool) // �� �����̰� �ϴ� ��ų��
    {
        stun = true;
        while (cool > 0.0f)
        {
            state[0].text = "Stun";
            cool -= Time.deltaTime;
            yield return null;
        }
        stun = false;
    }

    IEnumerator Jumping(float cool, float cool2)
    {
        mySkill.canMove = false;
        while (cool > 0.0f)
        {
            state[0].text = "Jump";
            cool -= Time.deltaTime;
            yield return null;
        }
        Vector3 jumpPower = Vector3.up * JumpHeight;
        GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);

        while (cool2 > 0.0f)
        {
            cool2 -= Time.deltaTime;

            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            rigidbody.MovePosition(this.transform.position + dir * 1f * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, dir, 2f * Time.deltaTime);
            yield return null;
        }
        mySkill.canMove = true;
    }



}

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
                    C_Movement(); //상시실행
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
        canRun = true; //시작할 때 바로 뛸 수 있도록
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

    public void CheckGround() // 연속점프 방지, 점프를 땅에 있을 때만
    {

        //피봇 위치가 발끝이기 때문에 캐릭터 발이 땅에 붙어버리면 검출할 수 없기 때문에 (Vector3.up * 0.2f)로 살짝 올려서 레이를 쏨
        // Vector3.down 아래니까 아래로 쏴야 함
        // 얼마만큼의 거리에 레이저를 쏠건지 = 0.4f
        // = 레이저를 쏠건데 캐릭터의 발 끝보다 0.2 만큼 높은 위치에서 아래방향으로 쏠것이고 0.4 만큼만 레이저가 발사 될것이다
        // 이 길이 안에서 우리가 설정할 레이어가 검출이 되면 그 정보를 out hit 에 담아라

        // 이쪽 프로젝트로 옮기는 과정에서 원래 수치값(0.4f, 0.2f) 와 상이하게 해야하는 문제가 좀 있네요
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
        //스태미너 바의 밸류가 0에 근사치에 닿을 때
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
            // 시프트를 눌렀고, 이동거리가 있으며 canRun 이 false가 아닐 때
            {
                run = true;
                curAnim[0].SetBool("IsRunning", true);
                rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
            }
            else // 이동거리값이 0보다 작을 때 shift로 달리기 발동 안할 수 있도록
            {
                run = false;
                curAnim[0].SetBool("IsRunning", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !canRun)
        // 시프트 키를 떼었고, canRun 이 false일 때
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

        

        if (dir != Vector3.zero) //벡터의 제로가 아니라면 키 입력이 됨
        {
            // 앞으로 나아갈 때 + 방향으로 나아가는데 반대방향으로 나가가는 키를 눌렀을 때 -방향으로 회전하면서 생기는 오류를 방지하기위해 (부호가 서로 반대일 경우를 체크해서 살짝만 미리 돌려주는 코드) 어렵네요... 
            // 지금 바라보는 방향의 부호 != 나아갈 방향 부호
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) ||
                Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                //우리는 이동할 때 x 와 z 밖에 사용을 안하므로
                transform.Rotate(0, 1, 0); // 살짝만 회전
                //정 반대방향을 눌러도 회전안하는 버그 방지
                //미리 회전을 조금 시켜서 정반대인 경우를 제거
            }

            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
            // Slerp를 쓸지 Lerp를 쓸지 상의를 해봐야 할 것 같아용 
            // 캐릭터의 앞방향은 dir 키보드를 누른 방향으로 캐릭터 회전
            // Lerp를 쓰면 원하는 방향까지 서서히 회전
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

        if (this.transform.position.y < restrictedHeight) // 높이제한
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 jumpPower = Vector3.up * B_AddFloatPower;
                GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);
            }
        }
        
    }

    IEnumerator Stunned(float cool) // 못 움직이게 하는 스킬들
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

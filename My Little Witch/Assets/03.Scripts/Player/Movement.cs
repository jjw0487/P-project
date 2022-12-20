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

    [Header("Ray")]
    public Vector3 movePoint; // 이동 위치 저장
    public Camera mainCamera; // 메인 카메라
    public Vector3 MoveToRay;

    [Header("BroomMovement")]
    public float B_Speed = 10f;
    public float B_RotSpeed = 3f;
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
                Instantiate(Resources.Load("Effect/MagicAura"), this.transform.position + Vector3.up * 0.2f, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                rigidbody.drag = 0f;
                KK.SetActive(true);
                BR.SetActive(false);
                break;
            case ONWHAT.Broom:
                Instantiate(Resources.Load("Effect/MagicAura"), this.transform.position + Vector3.up * 0.2f, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
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
                C_Ray();
                if (mySkill.canMove && !stun)
                {
                    C_Movement(); //상시실행
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
        if (Input.GetKeyDown(KeyCode.L) && ground)
        {
            SwitchingCharacter();
        }

        

    }

    


    private void FixedUpdate()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        totalDist = dir.magnitude;

        // 움직임 관련해서는 나중에 옮겨주자
        if (onWhat == ONWHAT.Street)
        {
            // 목적지까지 거리가 0.1f 보다 멀다면
            if (Vector3.Distance(transform.position, movePoint) > 0.1f)
            {
                MoveToRay = (movePoint - transform.position).normalized * Speed;
                if (mySkill.canMove && !stun)
                {
                    rigidbody.MovePosition(this.transform.position + MoveToRay * Speed * Time.deltaTime);
                    Running();
                }
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
        }
        else
        {
            rigidbody.MovePosition(this.transform.position + dir * B_Speed * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, dir, B_RotSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            curAnim[0].SetTrigger("Land");
        }
    }

    public void CheckGround() // 연속점프 방지, 점프를 땅에 있을 때만
    {
        if(onWhat == ONWHAT.Street)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.localPosition + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer))
                //발끝에서 0.2만큼 올려서 아랫방향으로 0.4만큼 쏜다
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


    /////////////////////////////////Character/////////////////////////////////////////////////

    void C_Ray()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                movePoint = raycastHit.point;
                //Debug.Log("movePoint : " + movePoint.ToString());
                //Debug.Log("맞은 객체 : " + raycastHit.transform.name);
            }
        }
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
                rigidbody.MovePosition(this.gameObject.transform.position + MoveToRay * dashSpeed * Time.deltaTime);
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

    void C_Movement()
    {

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

        if (Input.GetKeyDown(KeyCode.Space) && ground)
        {
            state[0].text = "Jump";
            C_Jump();
        }
    }
    void C_Jump()
    {
        curAnim[0].SetTrigger("Jump");
        StartCoroutine(Jumping(0.6f, 0.8f, 0.6f));
    }

    
    ///////////////////////////////////////////Broom//////////////////////////////////////
    
    void B_Movement()
    {

        B_DashnHeight(); //대시와 높이제한
        //Vector3 characterDir = transform.rotation * dir;
        if (dir.z < 0)
        {
            if (dir.x < 0)//left
            {
                curAnim[1].SetBool("IsTurningRight", true);
            }
            if (dir.x > 0) //right
            {
                curAnim[1].SetBool("IsTurningLeft", true);

            }
        }
        if (dir.z > 0)
        {
          
            if (dir.x < 0)//left
            {
                curAnim[1].SetBool("IsTurningLeft", true);
            }
            if (dir.x > 0) //right
            {
                curAnim[1].SetBool("IsTurningRight", true);
            }
        }
        
        if (dir.x == 0)//Idle
        {
            print("3");
            curAnim[1].SetBool("IsTurningLeft", false);
            curAnim[1].SetBool("IsTurningRight", false);
        }

    }

    void B_DashnHeight()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashPower = this.transform.forward * -Mathf.Log(1 / rigidbody.drag) * 20f;
            // drag 공기저항값을 역수로 뒤집어서 로그로 바꾸고 - 를 넣어줘서 값을 구한 후 우리가 구한 대시양을 곱해준다 < 자연스러운 대시를 위해(무슨 소리인지 모르겠다.)
            rigidbody.AddForce(dashPower, ForceMode.VelocityChange);
        }


        if (this.transform.position.y < restrictedHeight) // 높이제한
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 jumpPower = Vector3.up * B_AddFloatPower;
                rigidbody.AddForce(jumpPower, ForceMode.VelocityChange);
            }
        }
    }


    /////////////////////////////////////Coroutine//////////////////////////////////////////////////////////
    
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

    IEnumerator Jumping(float cool, float cool2, float cool3)
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
        {// 점프 중 일정 시간에만 움직임과 로테이션을 따로 설정
            cool2 -= Time.deltaTime;
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            rigidbody.MovePosition(this.transform.position + dir * 1f * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, dir, 2f * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, dir, 2f * Time.deltaTime);
            yield return null;
        }

       /* while (cool3 > 0.0f)
        {
            cool3 -= Time.deltaTime;

            yield return null;
        }
        */
        mySkill.canMove = true;
    }



}

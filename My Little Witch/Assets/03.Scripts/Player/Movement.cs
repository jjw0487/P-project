using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.AI;
using static Movement;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

[Serializable]
public struct PlayerStat
{
    public PlayerData orgData;
    public float curHP;
    public float curMP;
}


public class Movement : CharacterProperty
{
    [Header("Character")]
    public ONWHAT onWhat = ONWHAT.Street;
    public GameObject KK;
    public GameObject BR;
    public PlayerStat charStat;

    [Header("Ray")]
    public Vector3 movePoint; // 이동 위치 저장
    public Camera mainCamera; // 메인 카메라


    [Header("Movement")]
    public Animator[] curAnim;
    public float C_speed = 4f;
    public float C_dashSpeed = 7f;
    public float C_rotSpeed = 10f;
    public float C_JumpHeight = 3f;
    public LayerMask layer;
    public bool run;
    public bool canRun = true;
    public bool stun = false;
    public bool ground = true;


    [Header("BroomMovement")]
    private Vector3 dir = Vector3.zero;
    public float B_Speed = 10f;
    public float B_RotSpeed = 3f;
    public float B_AddFloatPower = 0.2f;
    public float B_restrictedHeight = 1000f;


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
                myAgent.enabled = true;
                Instantiate(Resources.Load("Effect/MagicAura"), this.transform.position + Vector3.up * 0.2f, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                myRigid.drag = 0f;
                myRigid.constraints = RigidbodyConstraints.FreezeAll;

                KK.SetActive(true);
                BR.SetActive(false);
                break;
            case ONWHAT.Broom:
                myAgent.SetDestination(transform.position);
                myAgent.enabled = false;
                Instantiate(Resources.Load("Effect/MagicAura"), this.transform.position + Vector3.up * 0.2f, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                myRigid.drag = 6f;
                myRigid.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                
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
                    C_Ray();
                    Running();
                }

                if(!mySkill.canMove || stun) //스킬이나 스턴이 걸리면 움직임 정지
                {
                    myAgent.SetDestination(transform.position);
                }

                if(Input.GetKeyDown(KeyCode.S)) // S키를 눌러 정지
                {
                    myAgent.SetDestination(transform.position);
                }
                break;
            case ONWHAT.Broom:
                B_Movement();
                B_DashnHeight(); //대시와 높이제한
                break;
        }
    }

    private void Awake()
    {
        ChangeState(ONWHAT.Street);
        mainCamera = Camera.main;
    }

    void Start()
    {
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
        //totalDist = dir.magnitude;

        // 움직임 관련해서는 나중에 옮겨주자
        if (onWhat == ONWHAT.Broom)
        {
            myRigid.MovePosition(this.transform.position + dir * B_Speed * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, dir, B_RotSpeed * Time.deltaTime);
        }
       
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            curAnim[0].SetTrigger("Land");
        }
    }*/

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 100f, 1 << LayerMask.NameToLayer("Ground")))
            {
                movePoint = hitData.point;
                //print(movePoint);
                if (mySkill.canMove && !stun)
                {
                    myAgent.SetDestination(movePoint);
                   
                }
            }
        }

        /*rigidbody.position = Vector3.MoveTowards(transform.position, movePoint, Speed * Time.deltaTime);
        rigidbody.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed);*/

        /*totalRayDist = Vector3.Distance(transform.position, movePoint);
        Vector3 dir = movePoint - transform.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        Quaternion targetRot = Quaternion.LookRotation(dirXZ);*/

        if (myAgent.remainingDistance > 0.1f)
        {
            curAnim[0].SetBool("IsWalking", true);
            StateNotice();
        }
        else
        {
            curAnim[0].SetBool("IsWalking", false);
        }

    }

    void C_Movement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ground)
        {
            state[0].text = "Jump";
            C_Jump();
        }
    }

    void Running()
    {
        if (Mathf.Approximately(myStaminaSlider.value, 0.0f))
        //스태미너 바의 밸류가 0에 근사치에 닿을 때
        {
            canRun = false;
            if (myAgent.remainingDistance > 0.1f)
            {
                curAnim[0].SetBool("IsWalking", true);
            }
            else
            {
                curAnim[0].SetBool("IsWalking", false);
            }
            myAgent.speed = C_speed;
            curAnim[0].SetBool("IsRunning", false);

            run = false;
        }
        else
        {
            //canRun= true;
            if (Input.GetKey(KeyCode.LeftShift) && myAgent.remainingDistance > 0.1f && canRun)
            // 시프트를 눌렀고, 이동거리가 있으며 canRun 이 false가 아닐 때
            {
                run = true;
                myAgent.speed = C_dashSpeed;
                curAnim[0].SetBool("IsRunning", true);
                //rigidbody.position = Vector3.MoveTowards(transform.position, movePoint, dashSpeed * Time.deltaTime);
            }
            else // 이동거리값이 0보다 작을 때 shift로 달리기 발동 안할 수 있도록
            {
                run = false;
                myAgent.speed = C_speed;
                curAnim[0].SetBool("IsRunning", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !canRun)
        // 시프트 키를 떼었고, canRun 이 false일 때
        {
            canRun = true;
            myAgent.speed = C_speed;
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
            curAnim[1].SetBool("IsTurningLeft", false);
            curAnim[1].SetBool("IsTurningRight", false);
        }

    }

    void B_DashnHeight()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashPower = this.transform.forward * -Mathf.Log(1 / myRigid.drag) * 20f;
            // drag 공기저항값을 역수로 뒤집어서 로그로 바꾸고 - 를 넣어줘서 값을 구한 후 우리가 구한 대시양을 곱해준다 < 자연스러운 대시를 위해(무슨 소리인지 모르겠다.)
            myRigid.AddForce(dashPower, ForceMode.VelocityChange);
        }


        if (this.transform.position.y < B_restrictedHeight) // 높이제한
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 jumpPower = Vector3.up * B_AddFloatPower;
                myRigid.AddForce(jumpPower, ForceMode.VelocityChange);
               
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
        Vector3 jumpPower = Vector3.up * C_JumpHeight;
        GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);

        while (cool2 > 0.0f)
        {// 점프 중 일정 시간에만 움직임과 로테이션을 따로 설정
            cool2 -= Time.deltaTime;
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            myRigid.MovePosition(this.transform.position + dir * 1f * Time.deltaTime);
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

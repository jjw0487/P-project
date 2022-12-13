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
        canRun = true; //시작할 때 바로 뛸 수 있도록
    }

    void Update()
    {
        if(mySkill.canMove)
        {
            CharacterMovement(); //상시실행
            Running();
        }

    }
    public void OnDmg(float dmg)
    {
        myHPBar.HandleHP(dmg);
    }
    void Running()
    {
        /*if (run) // 달리기
        {
            rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
        }*/

        if (Mathf.Approximately(myStaminaSlider.value, 1f))
        //스태미너 바의 밸류가 0에 근사치에 닿을 때
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
            // 시프트를 눌렀고, 이동거리가 있으며 canRun 이 false가 아닐 때
            {
                run = true;
                curAnim.SetBool("IsRunning", true);
                rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
            }
            else // 이동거리값이 0보다 작을 때 shift로 달리기 발동 안할 수 있도록
            {
                run = false;
                curAnim.SetBool("IsRunning", false);
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
            //Lerp를 쓰면 원하는 방향까지 서서히 회전
        }
    }

    

}

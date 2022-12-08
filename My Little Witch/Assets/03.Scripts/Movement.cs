using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    new Rigidbody rigidbody;
    public Animator curAnim;

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
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMovement(); //상시실행

        if (Input.GetKey(KeyCode.LeftShift) && totalDist > 0.0f)
        {
            run = true;
            Running();
        }
        else
        {
            run = false;
            curAnim.SetBool("IsRunning", false);
        }


    }

    void CharacterMovement()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        totalDist = dir.magnitude;

        rigidbody.MovePosition(this.transform.position + dir * Speed * Time.deltaTime);

        if (totalDist > 0.0f)
        {
            curAnim.SetBool("IsWalking", true);
        }
        if (totalDist <= 0.0f)
        {
            curAnim.SetBool("IsWalking", false);
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

    void Running()
    {
        curAnim.SetBool("IsRunning", true);
        rigidbody.MovePosition(this.gameObject.transform.position + dir * dashSpeed * Time.deltaTime);
    }
}

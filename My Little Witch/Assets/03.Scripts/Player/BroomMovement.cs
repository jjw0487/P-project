using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomMovement : MonoBehaviour
{
    private Rigidbody rigidbody;

    [Header("Movement")]
    public float Speed = 6f;
    public float rotSpeed = 10f;
    private Vector3 dir = Vector3.zero;
    private float totalDist;

    [Header("Game Setting")]
    public Movement myCharacter;

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveTheBroom();
    }

    void MoveTheBroom()
    {

        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        totalDist = dir.magnitude;

        rigidbody.MovePosition(this.transform.position + dir * Speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) ||
                Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                //우리는 이동할 때 x 와 z 밖에 사용을 안하므로
                transform.Rotate(0, 1, 0); // 살짝만 회전
                //정 반대방향을 눌러도 회전안하는 버그 방지
                //미리 회전을 조금 시켜서 정반대인 경우를 제거
            }

            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
            // 캐릭터의 앞방향은 dir 키보드를 누른 방향으로 캐릭터 회전
            // Lerp를 쓰면 원하는 방향까지 서서히 회전
        }
    }
}

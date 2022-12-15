using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomMovement : MonoBehaviour
{
/*    //private Rigidbody rigidbody;

    [Header("Movement")]
    public float Speed = 6f;
    public float rotSpeed = 10f;
    private Vector3 dir = Vector3.zero;
    private float totalDist;
    public float jumpHeight = 0.1f; //���� ����

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

        GetComponent<Rigidbody>().MovePosition(this.transform.position + dir * Speed * Time.deltaTime);
        

        // �����̼��� ���� �� �������� �ʰ� �غ���
        
        
        
        

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 jumpPower = Vector3.up * jumpHeight;
            GetComponent<Rigidbody>().AddForce(jumpPower, ForceMode.VelocityChange);
        }
    }
    private void FixedUpdate()
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
    }*/
}

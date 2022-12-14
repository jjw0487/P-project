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
        transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);

    }
}

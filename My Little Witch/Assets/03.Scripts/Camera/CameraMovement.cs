using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform myTarget;

    [SerializeField] private float lerpspeed;
    //[SerializeField] private Vector2 ZoomRange = new Vector2(2.0f, 11.0f);
    [SerializeField] private float ZoomSpeed;
    //[SerializeField] private float rotateSpeed = 10.0f;
    //Vector3 myDir = Vector3.zero;
   // float targetDist = 0.0f;
    //float dist = 0.0f;
    private void Start()
    {
        /*myDir = transform.position - myTarget.position;
        targetDist = dist = myDir.magnitude;
        myDir.Normalize();*/
    }
    void Update()
    {
       /* targetDist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        targetDist = Mathf.Clamp(targetDist, ZoomRange.x, ZoomRange.y);

        dist = Mathf.Lerp(dist, targetDist, Time.deltaTime * 5.0f);*/

        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(myTarget.transform.position, Vector3.up, Input.GetAxis("Mouse X") * 10f);
            //transform.position = myTarget.transform.position;
            transform.position = Vector3.Lerp(this.transform.position, myTarget.position, lerpspeed * Time.deltaTime);
        }
        else
        {
            //transform.position = myTarget.transform.position;
            transform.position = Vector3.Lerp(this.transform.position, myTarget.position, lerpspeed * Time.deltaTime);
        }

    }


}
using System.Collections.Generic;
using UnityEngine;
using static Movement;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform myTarget;
    [SerializeField] private float lerpspeed;
  
    public enum CAMTYPE { OnStreet, OnBroom, Dead }
    public CAMTYPE camType = CAMTYPE.OnStreet;

    [Header("OnBroom")]
    [SerializeField] private float rotX; //마우스 인풋값
    [SerializeField] private float rotY;
    [SerializeField] private float sensitivity = 30.0f;
    [SerializeField] private float clampAngle = 40.0f;
    public void ChangeState(CAMTYPE what)
    {
        //if (onWhat == what) return;
        camType = what;
        switch (camType)
        {
            case CAMTYPE.OnStreet:
                break;
            case CAMTYPE.OnBroom:
                break;
            case CAMTYPE.Dead:
                break;
        }
    }
    public void StateProcess()
    {
        switch (camType)
        {
            case CAMTYPE.OnStreet:
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
                break;
            case CAMTYPE.OnBroom:
                CameraRotation();
                //transform.position = Vector3.MoveTowards(transform.position, myTarget.position, lerpspeed * Time.deltaTime);
                transform.position = Vector3.Lerp(this.transform.position, myTarget.position, lerpspeed * Time.deltaTime);
                break;
            case CAMTYPE.Dead:
                break;
        }
    }


    private void Start()
    {
        //초기화
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
    }
    void Update()
    {
        StateProcess();
    }


    void CameraRotation()
    {
        //프레임마다 인풋을 받음
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        // 마우스 상,하 이동으로 y 축의 이동을 받음 -()이유는 화면 위 최대로 마우스를 올렸을 때 내려가는것을 방지
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle); // 최소값은 - 40, 최대값은 40
        //회전
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

}
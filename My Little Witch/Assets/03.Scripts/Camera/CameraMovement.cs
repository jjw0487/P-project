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
    [SerializeField] private float rotX; //���콺 ��ǲ��
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
        //�ʱ�ȭ
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
    }
    void Update()
    {
        StateProcess();
    }


    void CameraRotation()
    {
        //�����Ӹ��� ��ǲ�� ����
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        // ���콺 ��,�� �̵����� y ���� �̵��� ���� -()������ ȭ�� �� �ִ�� ���콺�� �÷��� �� �������°��� ����
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle); // �ּҰ��� - 40, �ִ밪�� 40
        //ȸ��
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

}
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform myTarget;
    [SerializeField] private float lerpspeed;
    [SerializeField] private Vector2 ZoomRange = new Vector2(2.0f, 11.0f);
    [SerializeField] private float ZoomSpeed;
    //[SerializeField] private float rotateSpeed = 10.0f;

    Vector3 myDir = Vector3.zero;
    float targetDist = 0.0f;
    float dist = 0.0f;


    void Start()
    {
        myDir = transform.position - myTarget.position;
        targetDist = dist = myDir.magnitude;
        myDir.Normalize();

    }

    void Update()
    {
        targetDist -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        targetDist = Mathf.Clamp(targetDist, ZoomRange.x, ZoomRange.y);
        dist = Mathf.Lerp(dist, targetDist, Time.deltaTime * 5.0f);

        this.transform.position = Vector3.Lerp(this.transform.position, myTarget.transform.position + myDir * dist, lerpspeed);

/*
        Vector3 rot = myTarget.transform.rotation.eulerAngles; // 현재 카메라의 각도를 Vector3로 반환
        rot.y += Input.GetAxis("Mouse X") * rotateSpeed; // 마우스 X 위치 * 회전 스피드
                                                         //rot.x += -1 * Input.GetAxis("Mouse Y") * rotateSpeed; // 마우스 Y 위치 * 회전 스피드
        Quaternion q = Quaternion.Euler(rot); // Quaternion으로 변환
        this.transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.5f); // 자연스럽게 회전
*/
    }

}
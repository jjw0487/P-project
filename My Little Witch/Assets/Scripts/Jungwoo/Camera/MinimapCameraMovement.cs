//작성자 : 이현호
//설명 : 

using UnityEngine;

public class MinimapCameraMovement : MonoBehaviour
{
    [Header("ObjectToFollow")] public Transform objectToFollow;
    public Transform camRot;

    [Header("Controller")] public float followSpeed = 10.0f;
    private float rotY;

    void Update()
    {
        Vector3 pos = objectToFollow.position;
        pos.y = transform.position.y;
        transform.position = pos;
        
        //회전
        rotY = camRot.rotation.eulerAngles.y;
        Quaternion rot = Quaternion.Euler(90, rotY, 0);
        transform.rotation = rot;
    }
}
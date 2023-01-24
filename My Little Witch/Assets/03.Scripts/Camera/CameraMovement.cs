using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform myTarget;
    [SerializeField] private float lerpspeed;
    [SerializeField] private Vector2 ZoomRange = new Vector2(2.0f, 11.0f);
    [SerializeField] private float ZoomSpeed;
    //[SerializeField] private float rotateSpeed = 10.0f;

    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(myTarget.transform.position, Vector3.up, Input.GetAxis("Mouse X") * 10f);
            transform.position = myTarget.transform.position;
        }
        else
        {
            transform.position = myTarget.transform.position;
        }
        
    }


}
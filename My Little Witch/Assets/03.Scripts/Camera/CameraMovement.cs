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
    public Vector3 offset;



    void Start()
    {
    }

    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(myTarget.transform.position, Vector3.up, Input.GetAxis("Mouse X") * 10f);
        }
        else
        {
            transform.position = myTarget.transform.position;
        }
        
    }


}
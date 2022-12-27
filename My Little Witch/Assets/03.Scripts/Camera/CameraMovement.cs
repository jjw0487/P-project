using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform myTarget;
    [SerializeField] private float lerpspeed;
    [SerializeField] private Vector2 ZoomRange = new Vector2(5.7f, 11.0f);
    [SerializeField] private float ZoomSpeed;

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
    }

}
// 혼자 하다 도저히 안돼서 수업내용 FollowingCamera 참고

using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform myTarget;
    // Neck을 따라가게 하는게 가장 자연스러웠습니다.

    [SerializeField] private float lerpspeed;
    [SerializeField] private Vector2 ZoomRange = new Vector2(5.7f, 11.0f);
    [SerializeField] private float ZoomSpeed;

    Vector3 myDir = Vector3.zero;
    float targetDist = 0.0f;
    float dist = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        myDir = transform.position - myTarget.position;
        targetDist = dist = myDir.magnitude;
        myDir.Normalize();

    }

    // Update is called once per frame
    void Update()
    {
        targetDist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        targetDist = Mathf.Clamp(targetDist, ZoomRange.x, ZoomRange.y);

        dist = Mathf.Lerp(dist, targetDist, Time.deltaTime * 5.0f);

        // 유니티에서 lerpspeed 값을 1 로 바꾸면 딱딱하게 따라갈 수 있도록 변경 가능
        this.transform.position = Vector3.Lerp(this.transform.position, myTarget.transform.position + myDir * dist, lerpspeed);
    }

}
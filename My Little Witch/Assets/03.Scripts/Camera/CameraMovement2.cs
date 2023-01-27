using UnityEngine;


// 0109


public class CameraMovement2 : MonoBehaviour
{
    public Transform myPlayer;
    public Vector3 offset;

    void Update()
    {
        transform.position = myPlayer.position + offset;
    }

    /*[Header("ObjectToFollow")] public Transform objectToFollow;

    [Header("Controller")] public float followSpeed = 10.0f;
    public float sensitivity = 30.0f;
    public float clampAngle = 40.0f;
    public float smoothness = 10.0f;
    public Vector3 finalDir; // 최종방향
    public float maxDistance;

  *//*  [Header("Inventory")] public GameObject myInventory;
    public GameObject myESC;
    public GameObject mySave;
    public GameObject myLoad;
    public GameObject askingUI;
    public GameObject myStatus;
    public GameObject Shop;*//*


    [Header("MainCamera")] public Transform realCamera;


    private Vector3 dirNormalized;
    private float rotX; //마우스 인풋값
    private float rotY;

    //방해물이 많아 껏습니다. (주변 식물들을 마땅한 레이어가 없어서 그라운드로 설정)

    void Start()
    {
        //초기화
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        //벡터값 초기화
        dirNormalized = realCamera.localPosition.normalized;
    }

    void Update()
    {
        CameraRotation();
        //OnCursor();
        // 카메라가 오브젝트를 따라감
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);
        // 최종 정해진 방향
        finalDir = transform.TransformPoint(dirNormalized * maxDistance); //방향 * 최대거리
    }

    *//*void OnCursor()
    {
        if (myInventory.activeSelf || myESC.activeSelf || mySave.activeSelf || myLoad.activeSelf || askingUI.activeSelf || myStatus.activeSelf || Shop.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                CameraRotation();
            }
        }
    }*//*

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
    }*/
}

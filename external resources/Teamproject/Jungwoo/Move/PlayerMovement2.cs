//작성자 : 전정우
//설명 :
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    private Vector3 dir;
    public Transform player;
    public Transform cameraOrigin;
    float curRot;

    void Start()
    {
        curRot = cameraOrigin.localRotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if (dir != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            player.rotation = Quaternion.Euler(0, targetAngle, 0) * cameraOrigin.transform.rotation;
        }

        curRot += Input.GetAxis("Mouse X") * 1.0f;
        cameraOrigin.rotation = Quaternion.Euler(0, curRot, 0);

        Vector3 inputDirection = cameraOrigin.transform.rotation * dir;

        this.gameObject.transform.Translate(inputDirection * 10.0f * Time.deltaTime);
    }
}
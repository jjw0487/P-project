using UnityEngine;

public class NotificationController : MonoBehaviour
{
    public TMPro.TMP_Text content;

    public void GetText(string ctt)
    {
        content.text = ctt + " Acquired";
    }

    public void AnimEvent() // �ִ� �̺�Ʈ�� ����
    {
        Destroy(this.gameObject);
    }

}

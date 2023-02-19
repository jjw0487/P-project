using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public TMPro.TMP_Text content;

    public void GetText(string ctt)
    {
        content.text = ctt + " Acquired";
    }

    public void AnimEvent() // 애님 이벤트로 삭제
    {
        Destroy(this.gameObject);
    }

}

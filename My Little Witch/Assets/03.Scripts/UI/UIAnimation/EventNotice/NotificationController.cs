using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public TMPro.TMP_Text content;
    void Start()
    {
        content = null;
        Destroy(this.gameObject, 4f);
    }

    public void GetText(string ctt)
    {
        content.text = ctt + " item Acquired";
    }

}

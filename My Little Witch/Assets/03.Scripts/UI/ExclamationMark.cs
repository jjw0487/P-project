using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExclamationMark : MonoBehaviour
{
    public Transform myTarget;
    private void Start()
    {
        Destroy(this.gameObject, 2f);
    }

    void Update()
    {
        if (myTarget != null)
        {
            //카메라에 레이를 쏴서 카메라 포지션에 한 30f 만큼 떨어져 있으면 작동 안하도록 해볼까?
            //Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);
            transform.rotation = Quaternion.LookRotation((Camera.main.transform.position-this.transform.position).normalized);
            transform.position = myTarget.position;
        }
    }

}

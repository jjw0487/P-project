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
            //ī�޶� ���̸� ���� ī�޶� �����ǿ� �� 30f ��ŭ ������ ������ �۵� ���ϵ��� �غ���?
            //Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);
            transform.rotation = Quaternion.LookRotation((Camera.main.transform.position-this.transform.position).normalized);
            transform.position = myTarget.position;
        }
    }

}

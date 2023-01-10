using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHP : MonoBehaviour
{
    public Transform myTarget;
    public Slider myBar;

    void Update()
    {
        if (myTarget != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);    // 3���� �������� ���� ��ǥ���� ��ǥ�� 2D ȭ����� ��ġ���� �ٲ��ش�.
            if (pos.z < 0.0f)
            {
                transform.position = new Vector3(0, 10000, 0);
            }
            else
            {
                transform.position = pos;
            }
            transform.position = pos;

        }
    }
    
}

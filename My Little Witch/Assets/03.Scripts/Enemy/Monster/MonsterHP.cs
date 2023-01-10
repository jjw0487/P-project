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
            Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);    // 3차원 공간안의 월드 좌표값을 좌표를 2D 화면상의 위치으로 바꿔준다.
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

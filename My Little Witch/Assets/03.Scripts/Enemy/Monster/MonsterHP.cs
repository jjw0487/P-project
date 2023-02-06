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
            //카메라에 레이를 쏴서 카메라 포지션에 한 30f 만큼 떨어져 있으면 작동 안하도록 해볼까?
            Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);    
            // 3차원 공간안의 월드 좌표값을 좌표를 2D 화면상의 위치로.
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

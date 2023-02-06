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
            //ī�޶� ���̸� ���� ī�޶� �����ǿ� �� 30f ��ŭ ������ ������ �۵� ���ϵ��� �غ���?
            Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);    
            // 3���� �������� ���� ��ǥ���� ��ǥ�� 2D ȭ����� ��ġ��.
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

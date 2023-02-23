using UnityEngine;

public class MarkController : MonoBehaviour
{
    public Transform myTarget;
    void Update()
    {
        if (myTarget != null)
        {
            //ī�޶� ���̸� ���� ī�޶� �����ǿ� �� 30f ��ŭ ������ ������ �۵� ���ϵ��� �غ���?
            //Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);
            transform.rotation = Quaternion.LookRotation((Camera.main.transform.position - this.transform.position).normalized);
            transform.position = myTarget.position;
        }
    }
}

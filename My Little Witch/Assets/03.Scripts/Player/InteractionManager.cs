using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Movement plyer;
    [SerializeField] private GameObject interactionUI;
    public TMPro.TMP_Text[] dialogue;
    private GameObject exObj;
    public void GetType(float cool, int type, Transform lookAt)
    {
        // SceneData.Inst.myPlayer.OnInteraction <= �Ұ��� ���� ī�޶� �ִϸ����Ϳ��� event�� ó�� �� (�ٸ� �ִϸ��̼��� ȣ��)
        // cool-> �� �� �Ŀ� �ٸ� ȭ����ȯ�� ����ų������

        if(type == 1)
        {
            // type 1�� -> n�� �� anykey �ް� ȭ�� Ǯ����
            exObj = Instantiate(Resources.Load("UI/ExMark")) as GameObject;
            exObj.GetComponent<ExclamationMark>().myTarget = plyer.InteractionUIPos;
            plyer.GetInteraction(lookAt);
            Camera.main.transform.parent.GetComponent<Animator>()?.SetTrigger("Interaction");
            interactionUI.SetActive(true);
            StartCoroutine(OnInteration(cool));
        }
        else if(type == 2)
        {
            // 2�� -> NPC�� ��ȭ
        }
    }


    public IEnumerator OnInteration(float cool)
    {
        yield return new WaitForSeconds(cool);

        print("ã��");
        // �ƹ�Ű�� �����ÿ� �޼��� ȭ�鿡 ȣ��
        while (SceneData.Inst.myPlayer.OnInteraction == true)
        {            
            if(Input.anyKey)
            {
                Camera.main.transform.parent.GetComponent<Animator>()?.SetTrigger("AsBefore");
                interactionUI.SetActive(false);
                SceneData.Inst.myPlayer.OnInteraction = false;
                yield break;
            }
            yield return null;
        }
    }

}

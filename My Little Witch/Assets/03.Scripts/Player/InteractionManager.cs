using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private Player myPlyer;
    [SerializeField] private GameObject interactionUI;
    public TMPro.TMP_Text[] dialogue;
    private GameObject exObj;


    private void Start()
    {
        myPlyer = SceneData.Inst.myPlayer;
    }

    public void GetType(float cool, int type, Transform lookAt)
    {
        
        // cool-> �� �� �Ŀ� �ٸ� ȭ����ȯ�� ����ų������

        if(type == 1)
        {
            // type 1�� -> n�� �� anykey �ް� ȭ�� Ǯ����
            exObj = Instantiate(Resources.Load("UI/ExMark")) as GameObject;
            exObj.GetComponent<ExclamationMark>().myTarget = myPlyer.InteractionUIPos;
            myPlyer.GetInteraction(lookAt);
            Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("Interaction");
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

        // �ƹ�Ű�� �����ÿ� �޼��� ȭ�鿡 ȣ��
        while (SceneData.Inst.myPlayer.OnInteraction == true)
        {            
            if(Input.anyKey)
            {
                SceneData.Inst.dialogueManager.EndDialogue();
                yield break;
            }
            yield return null;
        }
    }

}

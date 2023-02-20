using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : PointerCheck
{
    private Player myPlyer;
    [SerializeField] private GameObject interactionUI;
    public TMPro.TMP_Text[] dialogue;
    private GameObject exObj;

    private void Start()
    {
        myPlyer = SceneData.Inst.myPlayer;
    }

    public void GetType(float cool, Transform lookAt)
    {
        // cool-> �� �� �Ŀ� �ٸ� ȭ����ȯ�� ����ų������

        // type 1�� -> n�� �� anykey �ް� ȭ�� Ǯ����
        exObj = Instantiate(Resources.Load("UI/ExMark")) as GameObject;
        exObj.GetComponent<ExclamationMark>().myTarget = myPlyer.interactionUIPos;
        myPlyer.GetInteraction(lookAt);
        Camera.main.transform.parent.GetComponent<Animator>().SetBool("IsInteracting", true);
        StartCoroutine(OnInteration(cool));
        
    }


    public IEnumerator OnInteration(float cool)
    {
        yield return new WaitForSeconds(cool);

        // �ƹ�Ű�� �����ÿ� �޼��� ȭ�鿡 ȣ��
        while (SceneData.Inst.myPlayer.OnInteraction == true)
        {            
            if(Input.anyKey)
            {
                SceneData.Inst.dialogueManager.DM_EndDialogue();
                yield break;
            }
            yield return null;
        }
    }

}

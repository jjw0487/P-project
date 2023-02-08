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
        
        // cool-> 몇 초 후에 다른 화면전환을 일으킬것인지

        if(type == 1)
        {
            // type 1번 -> n초 후 anykey 받고 화면 풀어줌
            exObj = Instantiate(Resources.Load("UI/ExMark")) as GameObject;
            exObj.GetComponent<ExclamationMark>().myTarget = plyer.InteractionUIPos;
            plyer.GetInteraction(lookAt);
            Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("Interaction");
            StartCoroutine(OnInteration(cool));
        }
        else if(type == 2)
        {
            // 2번 -> NPC와 대화
        }
    }


    public IEnumerator OnInteration(float cool)
    {
        yield return new WaitForSeconds(cool);

        // 아무키나 누르시오 메세지 화면에 호출
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

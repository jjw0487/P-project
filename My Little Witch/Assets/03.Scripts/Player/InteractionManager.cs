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
        // SceneData.Inst.myPlayer.OnInteraction <= 불값은 현재 카메라 애니메이터에서 event로 처리 중 (다른 애니메이션의 호출)
        // cool-> 몇 초 후에 다른 화면전환을 일으킬것인지

        if(type == 1)
        {
            // type 1번 -> n초 후 anykey 받고 화면 풀어줌
            exObj = Instantiate(Resources.Load("UI/ExMark")) as GameObject;
            exObj.GetComponent<ExclamationMark>().myTarget = plyer.InteractionUIPos;
            plyer.GetInteraction(lookAt);
            Camera.main.transform.parent.GetComponent<Animator>()?.SetTrigger("Interaction");
            interactionUI.SetActive(true);
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

        print("찾아");
        // 아무키나 누르시오 메세지 화면에 호출
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

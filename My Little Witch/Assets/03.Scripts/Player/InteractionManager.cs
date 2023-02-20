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
        // cool-> 몇 초 후에 다른 화면전환을 일으킬것인지

        // type 1번 -> n초 후 anykey 받고 화면 풀어줌
        exObj = Instantiate(Resources.Load("UI/ExMark")) as GameObject;
        exObj.GetComponent<ExclamationMark>().myTarget = myPlyer.interactionUIPos;
        myPlyer.GetInteraction(lookAt);
        Camera.main.transform.parent.GetComponent<Animator>().SetBool("IsInteracting", true);
        StartCoroutine(OnInteration(cool));
        
    }


    public IEnumerator OnInteration(float cool)
    {
        yield return new WaitForSeconds(cool);

        // 아무키나 누르시오 메세지 화면에 호출
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("enter");
        SceneData.Inst.myPlayer.OnInventory = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("exit");
        SceneData.Inst.myPlayer.OnInventory = false;
    }
}

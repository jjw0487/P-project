using UnityEngine;
using UnityEngine.EventSystems;

public class PointerCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnInventory = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnInventory = false;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class PointerCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnInteraction = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnInteraction = false;
    }
}

//작성자 : 전정우
//설명 : 1025

using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public Tooltip tooltip;
    public GameObject myTool;

    public void OnPointerEnter(PointerEventData eventData)
    {
        myTool.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myTool.SetActive(false);
    }
}
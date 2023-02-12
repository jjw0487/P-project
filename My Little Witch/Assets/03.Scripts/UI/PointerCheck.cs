using UnityEngine;
using UnityEngine.EventSystems;

public class PointerCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SceneData.Inst.myPlayer.OnUI = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.SetAsLastSibling(); // 클릭하면 화면 제일 앞에 배치
        }
    }
}

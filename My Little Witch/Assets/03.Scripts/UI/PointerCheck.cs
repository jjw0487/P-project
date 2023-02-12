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
            this.transform.SetAsLastSibling(); // Ŭ���ϸ� ȭ�� ���� �տ� ��ġ
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlots : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private SkillData skillData;
    [SerializeField] private SkillSet skillSet;
    [SerializeField] private Image Image;


    void Start()
    {
        //previousImg = img; //이미지를 미리 저장해서 슬롯이 클리어 될 때 새로 만들어줘야 함
        if (this.GetComponent<SkillTab>() != null) // 아이템을 놓고 실험할 경우
        {
            skillData = this.GetComponent<SkillTab>().myData;
        }
        else if (this.GetComponent<SkillSet>() != null)
        {
            skillSet = this.GetComponent<SkillSet>();
        }
        else
        {
            skillData = null;
            skillSet = null;
        }
    }

    /*public void AddSkillData(SkillData data)
    {
        skillData = data;
        Image.sprite = skillData.sprite;
    }*/

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skillData != null)
        {
            DragImage.Inst.dragSkillData = skillData;
            DragImage.Inst.DragSetImage(Image);
            DragImage.Inst.transform.position = eventData.position;
        }

        if (skillSet != null)
        {
            DragImage.Inst.dragSkillSet = skillSet;
            DragImage.Inst.DragSetImage(Image);
            DragImage.Inst.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skillData != null)
        {
            DragImage.Inst.transform.position = eventData.position;
        }

        if (skillSet != null)
        {
            DragImage.Inst.transform.position = eventData.position;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Inst.SetColor(0);
        DragImage.Inst.dragSkillData = null;
    }

    public void OnDrop(PointerEventData eventData)
    { // 다른 슬롯 위치에 놓였을 때

        if (DragImage.Inst.dragSkillSet != null) { ChangeSlot(); }

    }


    private void ChangeSlot()
    {
        SkillData temp = skillData; // 아이템을 담을 공간을 만들고

        skillSet.AddSkillData(DragImage.Inst.dragSkillData);

        if (temp != null)
        {
            DragImage.Inst.dragSkillSet.AddSkillData(temp);
        }
        else
        {
            //DragImage.Inst.dragSkillSet.ClearSlot();
        }
    }

    private void ClearSlot()
    {
        skillData = null;
        Image.sprite = null;
    }
}

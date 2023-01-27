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
        //previousImg = img; //�̹����� �̸� �����ؼ� ������ Ŭ���� �� �� ���� �������� ��
        if (this.GetComponent<SkillTab>() != null) // �������� ���� ������ ���
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
    { // �ٸ� ���� ��ġ�� ������ ��

        if (DragImage.Inst.dragSkillSet != null) { ChangeSlot(); }

    }


    private void ChangeSlot()
    {
        SkillData temp = skillData; // �������� ���� ������ �����

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

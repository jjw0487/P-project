using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public SkillBook skillBook;
    public Button lvUpBtn;
    public SkillData myData;
    public TMPro.TMP_Text curLv;


    private void Start()
    {
        curLv.text = myData.level.ToString();
        if (skillBook.skillPoint <= 0 || this.myData.level >= 5)
        {
            lvUpBtn.interactable = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) { SceneData.Inst.mySkill.skillSetArray[0].AddSkillData(myData); }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragImage.Inst.dragSkillData = myData;
        DragImage.Inst.DragSetSprite(myData.sprite);
        DragImage.Inst.transform.position = eventData.position;
        DragImage.Inst.fromBook = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragImage.Inst.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragImage.Inst.SetColor(0);
        DragImage.Inst.dragSkillData = null;
    }

    public void OnLevelUpButton() // 버튼에서 on click 활성화 되려면 public 유지해 
    {
        skillBook.CalculateSkillPoint(-1);
        ++myData.level;
        curLv.text = myData.level.ToString();
        if (skillBook.skillPoint <= 0 || this.myData.level >= 5)
        {
            lvUpBtn.interactable = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = 0; i < SceneData.Inst.mySkill.skillSetArray.Length; ++i) // 슬롯 갯수만큼 반복
            {
                if (SceneData.Inst.mySkill.skillSetArray[i].myData == null) 
                {
                    for(int n = 0; n < SceneData.Inst.mySkill.skillSetArray.Length; ++n)
                    {
                        if (SceneData.Inst.mySkill.skillSetArray[n].myData != null &&
                            SceneData.Inst.mySkill.skillSetArray[n].myData.triggerName == myData.triggerName)
                        {
                            return;
                        }
                    }
                    SceneData.Inst.mySkill.skillSetArray[i].AddSkillData(this.myData);
                    break; // 조건검사에 걸린다면 반복문 탈출
                }
            }
        }
    }
}

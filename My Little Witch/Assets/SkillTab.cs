using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTab : MonoBehaviour
{
    public SkillBook skillBook;
    public Button lvUpBtn;
    public SkillData myData;
    public TMPro.TMP_Text curLv;

    private void Start()
    {
        // 레벨이 5거나 스킬 포인트가 없다면 버튼을 interactable = false
    }


    public void OnLevelUpButton() // 버튼에서 on click 활성화 되려면 public 유지해 
    {
        skillBook.CalculateSkillPoint(-1);
        //++myData.lv

        /*if(myData.lv == 5)
        {
            lvUpBtn.interactable = false;
        }*/

    }

}

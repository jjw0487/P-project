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
        // ������ 5�ų� ��ų ����Ʈ�� ���ٸ� ��ư�� interactable = false
    }


    public void OnLevelUpButton() // ��ư���� on click Ȱ��ȭ �Ƿ��� public ������ 
    {
        skillBook.CalculateSkillPoint(-1);
        //++myData.lv

        /*if(myData.lv == 5)
        {
            lvUpBtn.interactable = false;
        }*/

    }

}

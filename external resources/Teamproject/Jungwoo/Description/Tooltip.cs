//작성자 : 전정우
//설명 : 1025 

using UnityEngine;

public class Tooltip : MonoBehaviour
{
    //public TextMeshProUGUI nameTxt;
    //public TextMeshProUGUI descriptionTxt;
    public GameObject myTarget;
    private RectTransform rectTransform;

    Vector2 pos = new Vector2(10f, 150f);
    /*public void SetupTooltip(string name, string des, int atk)
    {
        //nameTxt.text = name;
        //descriptionTxt.text = des;

        //주석 걸린 코드 지우지 말아주세요 나중에 텍스트값으로 연결하려구요
    }*/

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //rectTransform.position = Input.mousePosition; //마우스 위치로 사용하고 싶을 땐 피봇 위치를 0으로 맞춰주세요

        rectTransform.transform.position = myTarget.transform.position;
    }
}
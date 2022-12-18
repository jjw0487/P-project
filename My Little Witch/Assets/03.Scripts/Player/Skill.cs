using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Movement myPlayer;
    public MagicGageBar myMagicGage;
    public Slider myMagicCircuit;
    public bool canMove = true;
    public bool canSkill = true;

    public ChangeCursor myCursor;

    void Start()
    {
        
    }

    void Update()
    {
        OnSkill();
    }

    public void OnSkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && canSkill && myMagicCircuit.value > 40f)
        {
            myPlayer.curAnim[0].SetTrigger("MagicSplash");
            GameObject obj = Instantiate(Resources.Load("Effect/Circle"), this.transform.position + Vector3.up, Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;
            myMagicGage.HandleMP(40f);
            StartCoroutine(Chill(2.8f, obj));
        }
    }


    public void Notification(bool can, bool cant)
    {
        if(!can)
        {
            // .text = " 마력이 부족합니다. "
        }
        else
        {
            // .text = " 스킬을 시전중에 있습니다. "
        }
        
    }




    IEnumerator Chill(float cool, GameObject obj) // 못 움직이게 하는 스킬들
    {
        canSkill = false;
        canMove = false;
        //Cursor.SetCursor(myCursor.cursorImg[1], Vector2.zero, CursorMode.ForceSoftware);

        float coolTime = cool;
        while (cool > 0.0f)
        {
            myPlayer.state[0].text = "Chill";
            cool -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        canMove = true; //시간이 끝나면
        canSkill = true;
        //Cursor.SetCursor(myCursor.cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
    }

    IEnumerator Cool(float cool) // 스킬 쿨타임
    {
        canSkill = false;
        float coolTime = cool;
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }
        //시간이 끝나면
        canSkill = true;
    }
}

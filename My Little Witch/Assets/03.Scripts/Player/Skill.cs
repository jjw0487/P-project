using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [Header("Player")]
    public GameObject myCharacter;
    public Movement myPlayer;
    public Transform rangeOfSkills;
    public GameObject SkillLimit;
    public bool canMove = true;
    public bool canSkill = true;
    public MagicGageBar myMagicGage;
    public Slider myMagicCircuit;

    [Header("Cursor")]
    public ChangeCursor myCursor;

    [Header("SkillSet")]
    public SkillSet[] skillSetArray;


    void Update()
    {
        OnSkill();
    }
    public void OnSkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && canSkill)
        {
            skillSetArray[0].PerformSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && canSkill)
        {
            skillSetArray[1].PerformSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && canSkill)
        {
            skillSetArray[2].PerformSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && canSkill)
        {
            skillSetArray[3].PerformSkill();
        }
    }
    public void Notification(bool can, bool cant)
    {
        if(!can)
        {
            // .text = " ������ �����մϴ�. "
        }
        else
        {
            // .text = " ��ų�� �����߿� �ֽ��ϴ�. "
        }
    }
    public IEnumerator Chill(float cool, GameObject obj) // �� �����̰� �ϴ� ��ų��
    {
        canSkill = false;
        canMove = false;
        //Cursor.SetCursor(myCursor.cursorImg[1], Vector2.zero, CursorMode.ForceSoftware);
        while (cool > 0.0f)
        {
            myPlayer.state[0].text = "Chill";
            cool -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        canMove = true; //�ð��� ������
        canSkill = true;
        //Cursor.SetCursor(myCursor.cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
    }

    public IEnumerator Cool(float cool) // ��ų ��Ÿ��
    {
        canSkill = false;
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }
        //�ð��� ������
        canSkill = true;
    }

    public void StopSkillCoroutine()
    {
        StopAllCoroutines();
    }


}

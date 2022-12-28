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
    public MagicGageBar myMagicGage;
    public Slider myMagicCircuit;
    public bool canMove = true;
    public bool canSkill = true;

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
        if (Input.GetKeyDown(KeyCode.Alpha1) && canSkill && myMagicCircuit.value > 40f)
        {
            myPlayer.curAnim[0].SetTrigger("MagicSplash");
            GameObject obj = Instantiate(Resources.Load("Effect/Circle"), myCharacter.transform.position + Vector3.up, Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;
            myMagicGage.HandleMP(40f);
            StartCoroutine(Chill(2.8f, obj));

            skillSetArray[0].Skill();
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
    IEnumerator Chill(float cool, GameObject obj) // �� �����̰� �ϴ� ��ų��
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
        canMove = true; //�ð��� ������
        canSkill = true;
        //Cursor.SetCursor(myCursor.cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
    }

    IEnumerator Cool(float cool) // ��ų ��Ÿ��
    {
        canSkill = false;
        float coolTime = cool;
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }
        //�ð��� ������
        canSkill = true;
    }


}

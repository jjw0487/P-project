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
            myPlayer.curAnim[0].SetInteger("SkillNum", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && canSkill)
        {
            skillSetArray[1].PerformSkill();
            myPlayer.curAnim[0].SetInteger("SkillNum", 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && canSkill)
        {
            skillSetArray[2].PerformSkill();
            myPlayer.curAnim[0].SetInteger("SkillNum", 2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && canSkill)
        {
            skillSetArray[3].PerformSkill();
            myPlayer.curAnim[0].SetInteger("SkillNum", 3);
        }
    }
    

    public IEnumerator Chill(float cool) // �� �����̰� �ϴ� ��ų��
    {
        canSkill = false;
        canMove = false;
        //Cursor.SetCursor(myCursor.cursorImg[1], Vector2.zero, CursorMode.ForceSoftware);

        while (cool > 0.0f)
        {
            myPlayer.state[0].text = "Chill";
            cool -= Time.deltaTime;
            if(myPlayer.stun)
            {
                canMove = true; 
                canSkill = true;
                yield break;
            }

            yield return null;
        }
        //Destroy(obj); // �׳� ����Ʈ�� ��ũ��Ʈ �޾Ƽ� �ð� ������ ������� ������� ����

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
    



}

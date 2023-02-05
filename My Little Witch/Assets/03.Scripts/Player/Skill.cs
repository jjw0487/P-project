using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [Header("Player")]
    public Movement myPlayer; // 'SkillSet' �� ����
    public GameObject rangeOfSkills; // 'SkillSet' �� ����
    public bool canMove = true; // 'Movement', 'SkillSet' �� ����
    public bool canSkill = true; // 'Movement', 'SkillSet' �� ����
    public MagicGageBar myMagicGage; // 'Movement', 'SkillSet' �� ����
    public Slider myMagicCircuit; // 'Movement', 'SkillSet' �� ����

    [Header("Cursor")]
    //public ChangeCursor myCursor;

    [Header("SkillSet")]
    public SkillSet[] skillSetArray;

    private void Awake()
    {

    }
    void Update()
    {
        OnSkill();
    }
    public void OnSkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && canSkill && skillSetArray[0].myData != null)
        {
            skillSetArray[0].PerformSkill();
            myPlayer.curAnim[0].SetInteger("SkillNum", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && canSkill && skillSetArray[1].myData != null)
        {
            skillSetArray[1].PerformSkill();
            myPlayer.curAnim[0].SetInteger("SkillNum", 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && canSkill && skillSetArray[2].myData != null)
        {
            skillSetArray[2].PerformSkill();
            myPlayer.curAnim[0].SetInteger("SkillNum", 2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && canSkill && skillSetArray[3].myData != null)
        {
            skillSetArray[3].PerformSkill();
            myPlayer.curAnim[0].SetInteger("SkillNum", 3);
        }
    }


    public IEnumerator Chill(float cool) // �ִϸ��̼� ���� ������ ���� <- SkillData.remainTime
    {
        canSkill = false; // -> SkillSet.cs
        canMove = false; // -> Movement.cs
        //Cursor.SetCursor(myCursor.cursorImg[1], Vector2.zero, CursorMode.ForceSoftware);
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            if (myPlayer.stun)
            {
                canMove = true;
                canSkill = true;
                yield break;
            }
            yield return null;
        }
        canMove = true;
        canSkill = true;
        //Cursor.SetCursor(myCursor.cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
    }


}

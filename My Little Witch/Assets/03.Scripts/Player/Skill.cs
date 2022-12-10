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
            myPlayer.curAnim.SetTrigger("MagicSplash");
            GameObject obj = Instantiate(Resources.Load("Effect/Circle"), this.transform.position + Vector3.up, Quaternion.identity) as GameObject;
            myMagicGage.HandleMP(40f);
            StartCoroutine(Chill(3.2f, obj));
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

        float coolTime = cool;
        while (cool > 0.0f)
        {
            myPlayer.state.text = "Chill";
            cool -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        canMove = true; //�ð��� ������
        canSkill = true;
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

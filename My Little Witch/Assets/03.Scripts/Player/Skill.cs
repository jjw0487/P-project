using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Movement myPlayer;
    public bool canMove = true;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            myPlayer.curAnim.SetTrigger("MagicSplash");
            GameObject obj = Instantiate(Resources.Load("Effect/Circle"), this.transform.position + Vector3.up, Quaternion.identity) as GameObject;
            StartCoroutine(Chill(3.2f, obj));
        }
         
    }

    IEnumerator Chill(float cool, GameObject obj) // �� �����̰� �ϴ� ��ų��
    {
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
    }

    IEnumerator Cool(float cool) // ��ų ��Ÿ��
    {
        float coolTime = cool;
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }
         //�ð��� ������
    }
}

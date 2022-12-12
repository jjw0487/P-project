using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicGageBar : MonoBehaviour
{
    [Header("Magic Circuit")] public Slider MagicBar;

    [Header("Skill")] public Skill Skill;

    private float curMP = 100f;
    private float maxMP = 100f;

    void Update()
    {
        
        HandleMP(0f);
    }

    public void HandleMP(float consume)
    {
        curMP = Mathf.Clamp(curMP, 0.1f, maxMP); // clamp ���� 0.1�� ��ż� delta�� �پ���� �ʴ� ������ ����
                                                 // ����Ʈ�� ������ ������ �������� �����ð� ��� �� �ٽ� �������� �ʴ� ������ ����

        curMP -= consume; //����
        curMP += 5 * Time.deltaTime; // ����

        //���� �ε巯�� ����
        MagicBar.value = Mathf.Lerp(MagicBar.value, curMP / maxMP * 100f, 30f * Time.deltaTime);
        // a �� b ������ t ��ŭ�� ���� ��ȯ
    }
}

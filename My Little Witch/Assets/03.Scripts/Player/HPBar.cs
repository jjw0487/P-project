using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [Header("HP Bar")] public Slider HPbar;


    private float curHP = 100f;
    private float maxHP = 100f;

    void Update()
    {
        HandleHP(0f);

        //�ڷ�ƾ�� ���� n�� �Ŀ� ü���� n�� ȸ���ϵ��� ������.
    }

    public void HandleHP(float consume)
    {
        curHP = Mathf.Clamp(curHP, 0.1f, maxHP); // clamp ���� 0.1�� ��ż� delta�� �پ���� �ʴ� ������ ����

        curHP -= consume; // ����
        curHP += 2 * Time.deltaTime; // ����

        //���� �ε巯�� ����
        HPbar.value = Mathf.Lerp(HPbar.value, curHP / maxHP * 100f, 10f * Time.deltaTime);
        // a �� b ������ t ��ŭ�� ���� ��ȯ
    }
}

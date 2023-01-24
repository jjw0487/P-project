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

        //코루틴을 만들어서 n초 후에 체력이 n씩 회복하도록 만들자.
    }

    public void HandleHP(float consume)
    {
        curHP = Mathf.Clamp(curHP, 0.1f, maxHP); // clamp 값을 0.1로 잠궈서 delta가 줄어들지 않는 현상을 방지

        curHP -= consume; // 감소
        curHP += 2 * Time.deltaTime; // 증가

        //바의 부드러운 증감
        HPbar.value = Mathf.Lerp(HPbar.value, curHP / maxHP * 100f, 10f * Time.deltaTime);
        // a 와 b 사이의 t 만큼의 값을 반환
    }
}

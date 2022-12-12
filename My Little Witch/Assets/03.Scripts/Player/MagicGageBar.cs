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
        curMP = Mathf.Clamp(curMP, 0.1f, maxMP); // clamp 값을 0.1로 잠궈서 delta가 줄어들지 않는 현상을 방지
                                                 // 시프트를 여러번 누르면 게이지가 일정시간 경과 후 다시 차오르지 않는 현상을 방지

        curMP -= consume; //감소
        curMP += 5 * Time.deltaTime; // 증가

        //바의 부드러운 증감
        MagicBar.value = Mathf.Lerp(MagicBar.value, curMP / maxMP * 100f, 30f * Time.deltaTime);
        // a 와 b 사이의 t 만큼의 값을 반환
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1104

public class InventoryUI : MonoBehaviour
{
    [Header("Options")]
    [Range(0, 10)]
    [SerializeField] private int horizontalSlotCount = 5; // 슬롯 가로 개수
    [Range(0, 10)]
    [SerializeField] private int verticalSlotCount = 3; // 슬롯 세로 개수
    [SerializeField] private float slotMargin = 8f; // 한 슬롯의 상하좌우 여백
    [SerializeField] private float contentAreaPadding = 20f; // 인벤토리 영역의 내부 여백
    [Range(32, 64)]
    [SerializeField] private float slotSize = 35f; // 각 슬롯의 크기

    [Header("Connected Objects")]
    [SerializeField] private RectTransform contentAreaRT; // 슬롯 위치
    [SerializeField] private GameObject slotUIPrefab; // 슬롯 원본 프리팹

    /// <summary>
    /// 지정된 개수만큼 슬롯 영역 내의 슬롯들 동적 생성
    /// </summary>
    
    private void InitSlots()
    {
        // 슬롯 프리팹 설정
        slotUIPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        //slotUIPrefab.TryGetComponent(out ItemSlotUI itemslot);


    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

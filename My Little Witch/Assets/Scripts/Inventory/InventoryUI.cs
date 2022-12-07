using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1104

public class InventoryUI : MonoBehaviour
{
    [Header("Options")]
    [Range(0, 10)]
    [SerializeField] private int horizontalSlotCount = 5; // ���� ���� ����
    [Range(0, 10)]
    [SerializeField] private int verticalSlotCount = 3; // ���� ���� ����
    [SerializeField] private float slotMargin = 8f; // �� ������ �����¿� ����
    [SerializeField] private float contentAreaPadding = 20f; // �κ��丮 ������ ���� ����
    [Range(32, 64)]
    [SerializeField] private float slotSize = 35f; // �� ������ ũ��

    [Header("Connected Objects")]
    [SerializeField] private RectTransform contentAreaRT; // ���� ��ġ
    [SerializeField] private GameObject slotUIPrefab; // ���� ���� ������

    /// <summary>
    /// ������ ������ŭ ���� ���� ���� ���Ե� ���� ����
    /// </summary>
    
    private void InitSlots()
    {
        // ���� ������ ����
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

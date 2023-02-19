using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] QuickSlots[] quickSlots;
    private Coroutine stratUsingQuickSlot = null;

    public void StartQuickSlot()
    {
        if (stratUsingQuickSlot == null) stratUsingQuickSlot = StartCoroutine(StartUsingQuickSlot());
    }
    IEnumerator StartUsingQuickSlot() // 퀵슬롯 사용 전까지는 아무것도 실행 안되도록.
    {
        print("시작");
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                quickSlots[0].UseQuickSlotItem();
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                quickSlots[1].UseQuickSlotItem();
            }
            yield return null;
        }
    }
}

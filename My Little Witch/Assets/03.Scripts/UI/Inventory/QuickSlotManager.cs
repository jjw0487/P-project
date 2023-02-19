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
    IEnumerator StartUsingQuickSlot() // ������ ��� �������� �ƹ��͵� ���� �ȵǵ���.
    {
        print("����");
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

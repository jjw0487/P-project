using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionReturn : MonoBehaviour
{
    private Slots _slot = null;
    public void ReturnDecision(Slots slot)
    {
        _slot = slot;
    }
    public void ReturnValue()
    {
        _slot.UseSingleItem();
        this.gameObject.SetActive(false);
    }
    public void Cancel()
    {
        _slot = null;
        this.gameObject.SetActive(false);
    }


    public void AcceptSaving() // ���̺� YES
    {
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false); // �θ���� ��� ����
    }


    public void CancelSaving() // ���̺� NO
    {
        this.gameObject.SetActive(false);
    }

    // ������ ���� ��
    public void ReturnThrowItemValue()
    {
        _slot.ThrowAwaySingleItem();
        this.gameObject.SetActive(false);
    }
}

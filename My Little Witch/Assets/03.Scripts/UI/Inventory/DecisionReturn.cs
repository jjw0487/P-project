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

    public void CancelSaving()
    {
        this.gameObject.SetActive(false);
    }

    // 아이템 버릴 때
    public void ReturnThrowItemValue()
    {
        _slot.ThrowAwaySingleItem();
        this.gameObject.SetActive(false);
    }
}

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


    public void AcceptSaving() // 세이브 YES
    {
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false); // 부모까지 모두 종료
    }


    public void CancelSaving() // 세이브 NO
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

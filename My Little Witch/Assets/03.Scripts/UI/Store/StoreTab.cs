using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreTab : MonoBehaviour, IPointerClickHandler
{
    public ItemData item;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                SceneData.Inst.Inven.usePanel[4].gameObject.SetActive(true);
                SceneData.Inst.Inven.usePanel[4].GetComponent<ItemAmountReturn>().GetStoreItemInfo(item);
            }
        }
    }
}

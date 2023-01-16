using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragImage : MonoBehaviour
{
    public static DragImage Inst;
    public Slots dragSlot;
    [SerializeField]private Image itemImage;

    private void Awake()
    {
        Inst = this;
    }

    public void DragSetImage(Image _itemImage)
    {
        itemImage.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }


}

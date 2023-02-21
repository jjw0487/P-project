using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData", order = 4)]

public class ItemData : ScriptableObject
{
    public enum ItemType { Consumable, Material, Interactable, Equipment}

    [Header("Type")]
    public ItemType itemType;
    public string itemName;
    [TextArea(3, 10)]
    public string explanation;
    public Sprite sprite;

    [SerializeField]
    private GameObject Obj;
    public GameObject obj { get { return Obj; } }

    [SerializeField]
    private int Count;
    public int count { get{ return Count; } }

    [Header("Selective::Consumable")]
    [SerializeField]
    private int ValueType;
    public int valueType { get { return ValueType; } } // 회복류 아이템의 타입 설정 1:hp, 2:mp, 3:st, 장비는 옵션 1.공격력, 2.

    [SerializeField]
    private int Value; 
    public int value { get { return Value; } } // 회복량, 장비는 성능

    [SerializeField]
    private int CurrencyInStore;
    public int currencyInStore { get { return CurrencyInStore; } }

    public string itemNameInStore;
}

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
    public int valueType { get { return ValueType; } } // ȸ���� �������� Ÿ�� ���� 1:hp, 2:mp, 3:st, ���� �ɼ� 1.���ݷ�, 2.

    [SerializeField]
    private int Value; 
    public int value { get { return Value; } } // ȸ����, ���� ����

    [SerializeField]
    private int CurrencyInStore;
    public int currencyInStore { get { return CurrencyInStore; } }

    public string itemNameInStore;
}

using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData", order = 4)]

public class ItemData : ScriptableObject
{
    public enum ItemType { Consumable, Material, Interactable }

    [Header("Type")]
    public ItemType itemType;
    public string itemName;
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
    public int valueType { get { return ValueType; } } // 회복류 아이템의 타입 설정 1:hp, 2:mp, 3:st

    [SerializeField]
    private int Value;
    public int value { get { return Value; } } // 회복량

}

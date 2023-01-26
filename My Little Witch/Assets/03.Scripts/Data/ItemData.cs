using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static SkillData;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData", order = 4)]

public class ItemData : ScriptableObject
{
    public enum ItemType { Consumable, Material, Interactable }

    [Header("Type")]
    public ItemType itemType;
    public int value;
    public int valueType;


    new public string name;
    public Sprite sprite;
    public GameObject obj;
    public int number;
    public string explanation;
}

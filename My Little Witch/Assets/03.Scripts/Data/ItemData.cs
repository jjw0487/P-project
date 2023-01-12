using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData", order = 4)]

public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public GameObject obj;
    public int number;
}

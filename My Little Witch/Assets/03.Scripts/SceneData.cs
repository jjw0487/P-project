using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneData : MonoBehaviour
{
    public static SceneData Inst = null;

    public Movement myPlayer;

    public Transform HPBars;
    public Transform FloatingDmg;
    //public Transform Minimap;
    public Transform InvenSlot;
    public Transform EagleExitPos;
    public Inventory Inven;

    public TMPro.TMP_Text itemExplnation;

    private void Awake()
    {
        Inst = this;
    }

}

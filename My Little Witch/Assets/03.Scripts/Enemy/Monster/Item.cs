using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemInformation
{
    public ItemData orgData;
    public int curNumber;
}

public class Item : MonoBehaviour
{

    public ItemInformation myItem;
    public Transform inventory;

    private void OnMouseEnter()
    {
        this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 1.0f);
    }

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            this.transform.SetParent(inventory);
        }
    }

    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 0.0f);
    }
}

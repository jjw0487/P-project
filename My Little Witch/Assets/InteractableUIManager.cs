using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUIManager : PointerCheck
{
    private bool inven = false;
    // private bool status = false;
    // private bool skillbook = false;

    [SerializeField] private GameObject Inventory;
    //[SerializeField] private GameObject Status;
    //[SerializeField] private GameObject SkillBook;

    private void Update()
    {
        KeyCodes();
    }

    private void KeyCodes()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inven = !inven;

            if (inven) { Open(Inventory); }
            else { Close(Inventory); }

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            //status = !status;
            //if (inven) { Open(Status); }
            //else { Close(Status); }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //skillBool = !skillBook;
            //if (inven) { Open(SkillBook); }
            //else { Close(SkillBook); }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAll();
        }
    }

    private void Open(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void Close(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void CloseAll()
    {
        Close(Inventory);
        //Close(Status);
        //Close(SkillBook);
    }

}

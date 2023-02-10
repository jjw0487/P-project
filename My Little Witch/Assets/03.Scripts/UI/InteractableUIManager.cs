using UnityEngine;

public class InteractableUIManager : PointerCheck
{
    private bool inven = false;
    private bool questBook = false;
    private bool skillbook = false;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject QuestBook;
    [SerializeField] private GameObject SkillBook;

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
            skillbook = !skillbook;
            if (skillbook) { Open(SkillBook); }
            else { Close(SkillBook); }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            questBook = !questBook;
            if (questBook) { Open(QuestBook); }
            else { Close(QuestBook); }
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
        Close(QuestBook);
        Close(SkillBook);
    }

}

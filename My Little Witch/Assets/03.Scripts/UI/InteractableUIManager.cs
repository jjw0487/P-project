using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;

public class InteractableUIManager : MonoBehaviour
{
    [Header("Gold")]
    [SerializeField] private int Gold;
    public int gold { get { return Gold; } }
    [SerializeField] private TMPro.TMP_Text text;

    [Header("GameUI")]
    private bool inven = false;
    private bool questBook = false;
    private bool skillbook = false;
    private bool gameMenu = false;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject QuestBook;
    public GameObject SkillBook; // ���������� ��ų����Ʈ ����
    [SerializeField] private GameObject GameMenu;
    public GameObject Store; // npc ��ȣ�ۿ� ����


    private Stack<GameObject> stack;

    private void Start()
    {
        stack = new Stack<GameObject>();
        Gold = 0;
        text.text = Gold.ToString();
    }
    private void Update()
    {
        KeyCodes();
        if (Input.GetKeyDown(KeyCode.M))
        {
            Gold += 1000;
            text.text = Gold.ToString();
        }
    }


    // ������ȭ ����
    public void SetGold(int gold)
    {
        Gold += gold;
        text.text = Gold.ToString();
    }

    public void PurchaseItem(int paid)
    {
        Gold -= paid;
        text.text = Gold.ToString();
    }



    // ui ����
    private void KeyCodes()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inven = !inven;
            if (inven) { Open(Inventory);}
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
            CloseAll(stack);
        }
    }

    public void Open(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<Transform>().SetAsLastSibling();
        if (!stack.Contains(obj)) { stack.Push(obj); } // �� �� �̻� ���ÿ� �Ƚ��̵���
    }

    public void CloseAll(Stack<GameObject> values)
    {
        if(stack.Count != 0) // ���ÿ� ī��Ʈ�� 0�� �ƴ� ��쿡��
        {
            GameObject obj = values.Pop();
            obj.SetActive(false);
            SceneData.Inst.myPlayer.OnUI = false;
        }
        else
        {
            // ���̺�, ȯ�漳�� â ���
            gameMenu = !gameMenu;
            if (gameMenu) { GameMenu.SetActive(true);}
            else { GameMenu.SetActive(false); SceneData.Inst.myPlayer.OnUI = false; }
        }
    }

    public void Close(GameObject obj)
    {
        obj.SetActive(false);
        SceneData.Inst.myPlayer.OnUI = false; // ���콺�� ui ���� ������ �� Ŭ���� �� ��� false �� ��ó �ȵɶ��� ���
    }

    public void OpenQuestBookAfterDialogue() // ����Ʈ â�� ����� ����Ʈ�Ͽ� ����Ʈ�� start �� �� �ֵ���
    {
        QuestBook.SetActive(true);
        QuestBook.GetComponent<Transform>().SetAsLastSibling();
        if (!stack.Contains(QuestBook)) { stack.Push(QuestBook); } // �� �� �̻� ���ÿ� �Ƚ��̵���
    }

    public void OpenStore() // ����Ʈ â�� ����� ����Ʈ�Ͽ� ����Ʈ�� start �� �� �ֵ���
    {
        Store.SetActive(true);
        Inventory.SetActive(true);
        Store.GetComponent<Transform>().SetAsLastSibling();
        if (!stack.Contains(Store)) { stack.Push(Store); } // �� �� �̻� ���ÿ� �Ƚ��̵���
        if (!stack.Contains(Inventory)) { stack.Push(Inventory); }
    }




}

using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;

public class InteractableUIManager : MonoBehaviour
{
    private bool inven = false;
    private bool questBook = false;
    private bool skillbook = false;
    private bool gameMenu = false;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject QuestBook;
    [SerializeField] private GameObject SkillBook;
    [SerializeField] private GameObject GameMenu;

    private Stack<GameObject> stack;

    private void Start()
    {
        stack = new Stack<GameObject>();
    }
    private void Update()
    {
        KeyCodes();
    }
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

    private void Open(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<Transform>().SetAsLastSibling();
        if (!stack.Contains(obj)) { stack.Push(obj); } // �� �� �̻� ���ÿ� �Ƚ��̵���
    }

    private void CloseAll(Stack<GameObject> values)
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
            else { GameMenu.SetActive(false); }
        }
    }

    private void Close(GameObject obj)
    {
        obj.SetActive(false);
        SceneData.Inst.myPlayer.OnUI = false; // ���콺�� ui ���� ������ �� Ŭ���� �� ��� false �� ��ó �ȵɶ��� ���

    }

}

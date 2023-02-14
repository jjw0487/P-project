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
        if (!stack.Contains(obj)) { stack.Push(obj); } // 두 번 이상 스택에 안쌓이도록
    }

    private void CloseAll(Stack<GameObject> values)
    {
        if(stack.Count != 0) // 스택에 카운트가 0이 아닐 경우에만
        {
            GameObject obj = values.Pop();
            obj.SetActive(false);
            SceneData.Inst.myPlayer.OnUI = false;
        }
        else
        {
            // 세이브, 환경설정 창 띄움
            gameMenu = !gameMenu;
            if (gameMenu) { GameMenu.SetActive(true);}
            else { GameMenu.SetActive(false); }
        }
    }

    private void Close(GameObject obj)
    {
        obj.SetActive(false);
        SceneData.Inst.myPlayer.OnUI = false; // 마우스가 ui 위에 존재할 때 클로즈 할 경우 false 가 미처 안될때를 대비

    }

}

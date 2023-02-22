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
    public GameObject SkillBook; // 레벨업마다 스킬포인트 증가
    [SerializeField] private GameObject GameMenu;
    public GameObject Store; // npc 상호작용 참조


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


    // 게임재화 관리
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



    // ui 관리
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
        if (!stack.Contains(obj)) { stack.Push(obj); } // 두 번 이상 스택에 안쌓이도록
    }

    public void CloseAll(Stack<GameObject> values)
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
            else { GameMenu.SetActive(false); SceneData.Inst.myPlayer.OnUI = false; }
        }
    }

    public void Close(GameObject obj)
    {
        obj.SetActive(false);
        SceneData.Inst.myPlayer.OnUI = false; // 마우스가 ui 위에 존재할 때 클로즈 할 경우 false 가 미처 안될때를 대비
    }

    public void OpenQuestBookAfterDialogue() // 퀘스트 창을 띄워서 퀘스트북에 퀘스트가 start 될 수 있도록
    {
        QuestBook.SetActive(true);
        QuestBook.GetComponent<Transform>().SetAsLastSibling();
        if (!stack.Contains(QuestBook)) { stack.Push(QuestBook); } // 두 번 이상 스택에 안쌓이도록
    }

    public void OpenStore() // 퀘스트 창을 띄워서 퀘스트북에 퀘스트가 start 될 수 있도록
    {
        Store.SetActive(true);
        Inventory.SetActive(true);
        Store.GetComponent<Transform>().SetAsLastSibling();
        if (!stack.Contains(Store)) { stack.Push(Store); } // 두 번 이상 스택에 안쌓이도록
        if (!stack.Contains(Inventory)) { stack.Push(Inventory); }
    }




}

//작성자 : 전정우
//설명 :

using UnityEngine;
using UnityEngine.EventSystems;

public class EnhancementController : MonoBehaviour, IPointerEnterHandler
{

    public RectTransform rectTransform;

    public GameObject myPanel;
    public GameObject myPlayerUI; // 박영준 플레이어ui가 자꾸 가려서 껏다키기위함.
    public GameObject myInfo;
    public GameObject myUI;
    public GameObject myMap;
    public GameObject myQuest;
    public GameObject myAlert;
    public RectTransform myInventory;
    public GameObject setMyInventory;
    public bool isOpen = false;
    public static EnhancementController inst = null;


    private void Awake()
    {
        inst = this;

        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        myInventory = myInventory.GetComponent<RectTransform>();
        myInventory.localPosition = new Vector2(0f, 35f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!myPanel.activeSelf)
        {
            rectTransform.SetAsFirstSibling();
        }
    }

    public void OpenUpEnhancement()
    {
        myInventory.localPosition = new Vector2(200f, 35f);
        myUI.SetActive(true);
        InventoryController.Instance.ShowNumbertoUI();
        setMyInventory.SetActive(true);
        myMap.SetActive(false);
        myQuest.SetActive(false);
        myPlayerUI.SetActive(false);
        isOpen = true;
    }

   
    public void CloseEnhancement()
    {
        myInventory.localPosition = new Vector2(0f, 35f);
        myUI.SetActive(false);
        myAlert.SetActive(false);
        setMyInventory.SetActive(false);
        myInfo.SetActive(false);
        myMap.SetActive(true);
        myQuest.SetActive(true);
        myPlayerUI.SetActive(true);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    OpenUpEnhancement();
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseEnhancement();
        }
    }
  
}
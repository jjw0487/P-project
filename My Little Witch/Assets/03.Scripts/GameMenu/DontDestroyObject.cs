using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyObject : MonoBehaviour
{
    public static DontDestroyObject instance = null;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform camRot;
    [SerializeField] private GameObject[] ifSceneNeeds; // 0.GUI, 1.Player 2.Camera
    private Transform townPosAfterWarp;
    private Transform townPosBeforeWarp;
    private Transform dungeonPosAfterWarp;

    public bool isWarping = false; // runegate ������ �̿��� ���̵�

    void Awake() // called zero
    {
        if (null == instance)
        {
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;
            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }
    }

    private void OnEnable() // called first
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // called second
    {
        if (scene.name == "Opening")
        {
            // 0.GUI, 1.Player 2.Camera
            ifSceneNeeds[0].SetActive(false);
            ifSceneNeeds[1].SetActive(false);
            ifSceneNeeds[2].SetActive(false);

        }
        else if(scene.name == "Title")
        {
            // 0.GUI, 1.Player 2.Camera
            ifSceneNeeds[0].SetActive(false);
            ifSceneNeeds[1].SetActive(false);
            ifSceneNeeds[2].SetActive(false);
        }
        else if (scene.name == "Town")
        {
            SceneData.Inst.questManager = FindObjectOfType<QuestManager>();
            if (!isWarping) // initial scene
            {
                // 0.GUI, 1.Player 2.Camera
                ifSceneNeeds[0].SetActive(true);
                ifSceneNeeds[1].SetActive(true);
                ifSceneNeeds[2].SetActive(true);

                townPosBeforeWarp = GameObject.FindGameObjectWithTag("TownPosBeforeWarp").transform;
                playerPos.position = townPosBeforeWarp.position;
                playerPos.rotation = townPosBeforeWarp.rotation;
                camRot.rotation = Quaternion.Euler(30f, 90f, 0f);
                SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
                SceneData.Inst.myPlayer.myAgent.enabled = true; // ��ġ�� ���� �Ŀ� ���ش�.
            }

            if (isWarping) // from Dungeon
            {  
                townPosAfterWarp = GameObject.FindGameObjectWithTag("TownPosAfterWarp").transform;
                playerPos.position = townPosAfterWarp.position;
                playerPos.rotation = townPosAfterWarp.rotation;
                camRot.rotation = Quaternion.Euler(30f, 0f, 0f);
                SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
                SceneData.Inst.myPlayer.myAgent.enabled = true; // ��ġ�� ���� �Ŀ� ���ش�.
            }
           
        }
        else if (scene.name == "Dungeon")
        {
            dungeonPosAfterWarp = GameObject.FindGameObjectWithTag("DungeonPosAfterWarp").transform;
            playerPos.position = dungeonPosAfterWarp.position;
            playerPos.rotation = dungeonPosAfterWarp.rotation;
            camRot.rotation = Quaternion.Euler(30f, 45f, 0f);

            SceneData.Inst.talkSign.SetBool("IsOpen", false);  // press f key
            //SceneData.Inst.myPlayer.myAgent.enabled = true; // ��ġ�� ���� �Ŀ� ���ش�.
            SceneData.Inst.questManager = null; // ���������� ����Ʈ ��ȣ�ۿ� ����
        }
    }

    void OnDisable() // called when the game is terminated
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}

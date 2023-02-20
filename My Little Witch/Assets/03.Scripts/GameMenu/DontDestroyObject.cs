using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyObject : MonoBehaviour
{
    public static DontDestroyObject instance = null;
    public Transform playerPos;
    public Transform camPos;

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
        print("Scene on scene");

        if (scene.name == "Town")
        { 
            SceneData.Inst.questManager = FindObjectOfType<QuestManager>();
            SceneData.Inst.myPlayer.myAgent.enabled = true; // ��ġ�� ���� �Ŀ� ���ش�.
            /*ChangeState(State.Menu);
            GamePanel_Sliders[0].onValueChanged.AddListener((float v) => GraphicManager.Inst.fbrightness = v);
            GamePanel_Sliders[1].onValueChanged.AddListener((float v) => GraphicManager.Inst.fconstrast = v);
            GamePanel_Sliders[2].onValueChanged.AddListener((float v) => SoundManager.Inst.bgmVolume = v);
            GamePanel_Sliders[3].onValueChanged.AddListener((float v) => SoundManager.Inst.effectVolume = v);
            newGameSceneName = "testScene";
            faderAnim?.SetTrigger("FadeIn");
            DisableUI();*/
        }

        if (scene.name == "Dungeon")
        {
            playerPos.position = new Vector3(161.59f, 25.0f, 132.47f);
            SceneData.Inst.myPlayer.myAgent.enabled = true; // ��ġ�� ���� �Ŀ� ���ش�.
            SceneData.Inst.questManager = null; // ���������� ����Ʈ ��ȣ�ۿ� ����
            /*newGameSceneName = "GameStage2";
            faderAnim.SetTrigger("FadeIn");*/
        }
    }

    void OnDisable() // called when the game is terminated
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}

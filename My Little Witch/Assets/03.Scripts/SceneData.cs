using UnityEngine;

public class SceneData : MonoBehaviour
{
    public static SceneData Inst = null;

    public Player myPlayer;
    public Skill mySkill;

    public Transform HPBars;
    public Transform FloatingDmg;

    public Transform InvenSlot;
    public Transform EagleExitPos;

    public Inventory Inven;
    public TMPro.TMP_Text itemExplnation; // 인벤토리 내에 아이템 설명 란

    public InteractionManager interactionManager;
    public DialogueManager dialogueManager;
    public Animator talkSign;
    public QuestManager questManager;

    public MonsterSpawner monSpawner;


    private void Awake()
    {
        Inst = this;
    }

}

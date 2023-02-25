[System.Serializable]
public class SaveData
{
    //player
    public int level;
    public float hp;
    public float[] position; //플레이어 마지막 위치

    public float[] addedStat;

    //inventory
    public int existItemCount;
    public int[] itemCount;
    public int[] items;
    public int[] equipmentItems;
    public int indexer;

    //skill
    public int skillPoint;
    public int[] skillLevel;

    //gold
    public int gold;

    //quest
    public int[] questIndex;
    public int[] npcProgress;


    public SaveData(Player _player, Inventory _inventory, InteractableUIManager _gold, QuestManager _quest, SkillTab[] _skillTab)
    {
        #region 생성자를 오버로딩 한 이유
        //생성자는 클래스의 객체를 생성하는 역할을 한다.
        //클래스를 선언할 때 생성자를 구현하지 않아도 컴파일러가 자동으로 생성하지만 그럼에도 생성자를 직접 구현하는 이유는
        //객체의 필드를 원하는 값으로 초기화하려고 할 때 적합한 장소가 생성자이기 때문
        #endregion

        #region player ->
        level = _player.Level;
        hp = _player.CurHP;
        position = new float[3];
        position[0] = _player.transform.position.x;
        position[1] = _player.transform.position.y;
        position[2] = _player.transform.position.z;

        addedStat = new float[2]; //현재 sp 와 dp
        addedStat[0] = _player.AddedSP;
        addedStat[1] = _player.AddedDP;
        #endregion

        #region Inventory ->

        indexer = 0;//인덱서 초기화
        existItemCount = 0;

        for (int i = 0; i < _inventory.slotData.Length; i++)
        {
            if (_inventory.slotData[i].item != null)
            {
                existItemCount++; // 존재하는 아이템 개수를 센 후
            }
        }

        items = new int[existItemCount];
        itemCount = new int[existItemCount];


        for (int i = 0; i < _inventory.slotData.Length; i++) // 아이템 배치를 띄엄띄엄 해 놨을 수 도  있으므로 다 검사 해야한다.
        {
            if (_inventory.slotData[i].item != null)
            {
                items[indexer] = _inventory.slotData[i].item.myItem.orgData.itemId;
                indexer++;
            }
        }

        indexer = 0; //인덱서 초기화

        for (int i = 0; i < _inventory.slotData.Length; i++)
        {
            if (_inventory.slotData[i].item != null)
            {
                itemCount[indexer] = _inventory.slotData[i].item.curNumber;
                indexer++;
            }
        }

        existItemCount = 0; //다시 0으로 초기화

        for (int i = 0; i < _inventory.equipSlots.Length; i++)
        {
            if (_inventory.equipSlots[i].item != null)
            {
                existItemCount++; // 존재하는 아이템 개수를 센 후
            }
        }


        equipmentItems = new int[existItemCount];
        indexer = 0;//인덱서 초기화

        for (int i = 0; i < _inventory.equipSlots.Length; i++)
        {
            if (_inventory.equipSlots[i].item != null)
            {
                equipmentItems[indexer] = _inventory.equipSlots[i].item.myItem.orgData.itemId;
                indexer++;
            }
        }
        #endregion

        #region Quest ->
        _quest.n = 0;

        if (_quest.questInProgress != null) // 진행중인 퀘스트 데이터
        {
            questIndex = new int[_quest.questInProgress.Count];

            for (int i = 0; i < questIndex.Length; i++)
            {
                questIndex[i] = _quest.questIndex[i];
            }
        }

        npcProgress = new int[_quest.npc.Length]; // npc 진행도 저장
        for (int i = 0; i < npcProgress.Length; i++)
        {
            npcProgress[i] = _quest.npc[i].progress;
        }

        #endregion

        #region Gold ->
        gold = _gold.gold;
        #endregion

        #region Skill ->
        skillLevel = new int[8];
        skillPoint = _skillTab[0].skillBook.skillPoint;
        for (int i = 0; i < _skillTab.Length; i++)
        {
            skillLevel[i] = _skillTab[i].myData.level;
        }
        #endregion

    }
}

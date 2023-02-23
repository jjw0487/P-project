using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SaveData
{
    //player
    public int level;
    public float hp;
    public float[] position; //�÷��̾� ������ ��ġ

    //inventory
    public int existItemCount;
    public int[] itemCount;
    public int[] items;
    public int[] equipmentItems;

    public int indexer;

    //skill
    public int[] skillLevel;

    //gold
    public int gold;

    //quest
    public int[] questIndex;
    public int[] npcProgress;

     
    public SaveData(Player _player, Inventory _inventory, InteractableUIManager _gold, QuestManager _quest, SkillTab[] _skillTab)
    {
        #region �����ڸ� �����ε� �� ����
        //�����ڴ� Ŭ������ ��ü�� �����ϴ� ������ �Ѵ�.
        //Ŭ������ ������ �� �����ڸ� �������� �ʾƵ� �����Ϸ��� �ڵ����� ���������� �׷����� �����ڸ� ���� �����ϴ� ������
        //��ü�� �ʵ带 ���ϴ� ������ �ʱ�ȭ�Ϸ��� �� �� ������ ��Ұ� �������̱� ����
        #endregion

        #region player ->
        level = _player.Level;
        hp = _player.CurHP;
        position = new float[3];
        position[0] = _player.transform.position.x;
        position[1] = _player.transform.position.y;
        position[2] = _player.transform.position.z;
        #endregion

        #region Inventory ->

        indexer = 0;//�ε��� �ʱ�ȭ
        existItemCount = 0;

        for (int i = 0; i < _inventory.slotData.Length; i++)
        {
            if (_inventory.slotData[i].item != null)
            {
                existItemCount++; // �����ϴ� ������ ������ �� ��
            }
        }

        items = new int[existItemCount];
        itemCount = new int[existItemCount];

        Debug.Log(items.Length);

        for (int i = 0; i < _inventory.slotData.Length; i++) // ������ ��ġ�� ������ �� ���� �� ��  �����Ƿ� �� �˻� �ؾ��Ѵ�.
        {
            if(_inventory.slotData[i].item != null)
            {
                items[indexer] = _inventory.slotData[i].item.myItem.orgData.itemId;
                indexer++;
            }
        }

        indexer = 0; //�ε��� �ʱ�ȭ

        for(int i = 0; i < _inventory.slotData.Length; i++)
        {
            if (_inventory.slotData[i].item != null)
            {  
                itemCount[indexer] = _inventory.slotData[i].item.curNumber;
                indexer++;
            }
        }

        existItemCount = 0; //�ٽ� 0���� �ʱ�ȭ

        for (int i = 0; i < _inventory.equipSlots.Length; i++)
        {
            if (_inventory.equipSlots[i].item != null)
            {
                existItemCount++; // �����ϴ� ������ ������ �� ��
            }
        }


        equipmentItems = new int[existItemCount];
        indexer = 0;//�ε��� �ʱ�ȭ

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

        if (_quest.questInProgress != null) // �������� ����Ʈ ������
        {
            questIndex = new int[_quest.questInProgress.Count];

            for(int i = 0; i < questIndex.Length; i++)
            {
                questIndex[i] = _quest.questIndex[i];
            }
        }

        npcProgress = new int[_quest.npc.Length]; // npc ���൵ ����
        for(int i = 0; i < npcProgress.Length; i++)
        {
            npcProgress[i] = _quest.npc[i].progress;
        }

        #endregion

        #region Gold ->
        gold = _gold.gold;
        #endregion

        #region Skill ->
        skillLevel = new int[8];
        for(int i = 0; i < _skillTab.Length; i++)
        {
            skillLevel[i] = _skillTab[i].curLevel;
        }
        #endregion


        // ������ �ٽ� ���� �� �� 3���� Ÿ�Ը� ���� �������� �˾ƺ���, �������� ��� �������� �˾ƺ���

    }
}

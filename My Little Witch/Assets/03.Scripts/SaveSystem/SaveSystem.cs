using System.IO; // the namespace we use whenever we want to work with files on our operationg system. => when creating or opening
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(Player _player, Inventory _inventory, InteractableUIManager _gold, QuestManager _quest, SkillTab[] _skillTab)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.chase";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(_player, _inventory, _gold, _quest, _skillTab);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log(path);
    }

    // 슬롯마다 폴더를 여러개 두고 <- 선택 파일에서 파일을 땡겨올 수 있도록.

    public static SaveData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.chase";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}

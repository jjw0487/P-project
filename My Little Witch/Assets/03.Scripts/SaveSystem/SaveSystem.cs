using UnityEngine;
using System.IO; // the namespace we use whenever we want to work with files on our operationg system. => when creating or opening
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting.FullSerializer;

public static class SaveSystem
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.chase";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(player);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log(path);
    }

    public static SaveData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.chase";
        if(File.Exists(path))
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

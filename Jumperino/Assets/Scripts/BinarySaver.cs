using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using static PlayerData;

public class BinarySaver
{
    public static PlayerData SaveDataOffline()
    {
        Debug.Log("Save offline initialized");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "PlayerData.hmml";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();
        data.maxScores = _maxScores;
        data.totalJumps = _totalJumps;
        data.totalCoins = _totalCoins;

        formatter.Serialize(stream, data);
        stream.Close();

        return data;
    }

    public static PlayerData LoadDataOffline()
    {
        Debug.Log("Load offline initialized");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "PlayerData.hmml";

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            _maxScores = data.maxScores;
            _totalJumps = data.totalJumps;
            _totalCoins = data.totalCoins;

            return data;
        }
        else
        {
            return SaveDataOffline();
        }
    }
}

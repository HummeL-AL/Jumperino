using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using static Global;
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
        data.nicknameEntered = _nicknameEntered;
        data.maxScores = _maxScores;
        data.totalJumps = _totalJumps;
        data.currentCoins = _currentCoins;
        data.totalCoins = _totalCoins;
        data.saveTime = Convert.ToUInt64(GetTime());

        data.unlockedPlayerSkins = _unlockedPlayerSkins;
        data.unlockedPlatformsSkins = _unlockedPlatformsSkins;
        data.unlockedBackgroundSkins = _unlockedBackgroundSkins;

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

            _nicknameEntered = data.nicknameEntered;
            _maxScores = data.maxScores;
            _totalJumps = data.totalJumps;
            _currentCoins = data.currentCoins;
            _totalCoins = data.totalCoins;

            _unlockedPlayerSkins = data.unlockedPlayerSkins;
            _unlockedPlatformsSkins = data.unlockedPlatformsSkins;
            _unlockedBackgroundSkins = data.unlockedBackgroundSkins;

            return data;
        }
        else
        {
            return SaveDataOffline();
        }
    }
}

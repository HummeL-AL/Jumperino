using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using static Global;
using static Spawner;
using static PlayerData;
using static GameController;

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
        data.ageEntered = _ageEntered;
        data.under13 = _under13;
        data.maxScores = _maxScores;
        data.totalJumps = _totalJumps;
        data.currentCoins = _currentCoins;
        data.totalCoins = _totalCoins;
        data.totalAdsWatched = _totalAdsWatched;
        data.totalGamesPlayed = _totalGamesPlayed;
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
            _ageEntered = data.ageEntered;
            _under13 = data.under13;
            _maxScores = data.maxScores;
            _totalJumps = data.totalJumps;
            _currentCoins = data.currentCoins;
            _totalCoins = data.totalCoins;
            _totalAdsWatched = data.totalAdsWatched;
            _totalGamesPlayed = data.totalGamesPlayed;

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

    public static PlayerSettings SaveSettingsOffline()
    {
        Debug.Log("Save settings offline initialized");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "PlayerSettings.hmml";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerSettings settings = new PlayerSettings();

        settings.playerSkin = pc.skin.name;
        settings.platformSkin = _platformSkin.name;
        settings.backgroundSkin = cam.GetComponent<Global>().skin.name;

        settings.jumpTime = pc.MaxTouchTime;
        settings.musicVolume = musicVolume;
        settings.soundVolume = soundVolume;

        formatter.Serialize(stream, settings);
        stream.Close();

        return settings;
    }

    public static PlayerSettings LoadSettingsOffline()
    {
        Debug.Log("Load settings offline initialized");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "PlayerSettings.hmml";

        if (File.Exists(path))
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                PlayerSettings settings = formatter.Deserialize(stream) as PlayerSettings;
                stream.Close();

                pc.Skin = Resources.Load<PlayerSkin>("Skins/Player/" + settings.playerSkin);
                _platformSkin = Resources.Load<PlatformSkin>("Skins/Platforms/" + settings.platformSkin);
                cam.GetComponent<Global>().Skin = Resources.Load<BackgroundSkin>("Skins/Background/" + settings.backgroundSkin);

                pc.MaxTouchTime = settings.jumpTime;
                musicVolume = settings.musicVolume;
                soundVolume = settings.soundVolume;

                return settings;
            }
            catch (Exception)
            {
                pc.skin = Resources.Load<PlayerSkin>("Skins/Player/Default");
                _platformSkin = Resources.Load<PlatformSkin>("Skins/Platforms/Default");
                cam.GetComponent<Global>().Skin = Resources.Load<BackgroundSkin>("Skins/Background/Default");

                pc.MaxTouchTime = 1f;
                musicVolume = 1f;
                soundVolume = 1f;
                return null;
            }
        }
        else
        {
            return SaveSettingsOffline();
        }
    }
}

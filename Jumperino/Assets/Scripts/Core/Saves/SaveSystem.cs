using UnityEngine;
using static Global;
using static PlayerData;
using static BinarySaver;
using static GameController;
using static DatabaseController;

public class SaveSystem : MonoBehaviour
{
    public static void TryToSaveData()
    {
        if (IsConnected())
        {
            Debug.Log("Trying to save offline...");
            SaveDataOffline();
            Debug.Log("Trying to sync data...");
            SyncData();
        }
        else
        {
            Debug.Log("Trying to save offline...");
            SaveDataOffline();
        }
    }

    public static void TryToLoadData()
    {
        if (IsConnected())
        {
            Debug.Log("Trying to sync data...");
            SyncData();
        }
        else
        {
            Debug.Log("Trying to load offline...");
            LoadDataOffline();
        }
    }

    public static void TryToSaveSettings()
    {
        Debug.Log("Trying to save offline...");
        SaveSettingsOffline();
    }

    public static void TryToLoadSettings()
    {
        Debug.Log("Trying to load offline...");
        LoadSettingsOffline();
    }

    public static async void SyncData()
    {
        Debug.Log("Sync initialized");
        PlayerData offlineData = LoadDataOffline();
        PlayerData onlineData = await LoadDataOnlineAsync();

        if (offlineData.totalJumps > onlineData.totalJumps)
        {
            Debug.Log("Offline data newer");
            AcceptData(offlineData);
            SaveDataOnline();
        }
        else
        {
            if (offlineData.totalJumps < onlineData.totalJumps)
            {
                Debug.Log("Online data newer");
                AcceptData(onlineData);
            }
            else
            {
                if (offlineData.saveTime > onlineData.saveTime)
                {
                    Debug.Log("Offline data newer");
                    AcceptData(offlineData);
                    SaveDataOnline();
                }
                else
                {
                    Debug.Log("Online data newer");
                    AcceptData(onlineData);
                }
            }
        }
    }

    public static void AcceptData(PlayerData acceptedData)
    {
        _maxScores = acceptedData.maxScores;
        _totalJumps = acceptedData.totalJumps;
        _currentCoins = acceptedData.currentCoins;
        _totalCoins = acceptedData.totalCoins;

        _unlockedPlayerSkins = acceptedData.unlockedPlayerSkins;
        _unlockedPlatformsSkins = acceptedData.unlockedPlatformsSkins;
        _unlockedBackgroundSkins = acceptedData.unlockedBackgroundSkins;

        UpdateScores();
    }
}
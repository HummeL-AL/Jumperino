using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using static Global;
using static PlayerData;

public static class DatabaseController
{
    public static int loadedScores;
    public static int loadedCoins;

    public static void SaveDataOnline()
    {
        Debug.Log("Save data online initialized");
        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;

        root.Child("Devices").Child(macAdress).Child("Nickname").SetValueAsync(_nickname);
        root.Child("Devices").Child(macAdress).Child("High score").SetValueAsync(_maxScores);
        root.Child("Devices").Child(macAdress).Child("Total jumps").SetValueAsync(_totalJumps);
        root.Child("Devices").Child(macAdress).Child("Current coins").SetValueAsync(_currentCoins);
        root.Child("Devices").Child(macAdress).Child("Total coins").SetValueAsync(_totalCoins);
        root.Child("Devices").Child(macAdress).Child("Skins").Child("PlayerSkins").SetValueAsync(_unlockedPlayerSkins);
        root.Child("Devices").Child(macAdress).Child("Skins").Child("PlatformsSkins").SetValueAsync(_unlockedPlatformsSkins);
        root.Child("Devices").Child(macAdress).Child("Skins").Child("BackgroundSkins").SetValueAsync(_unlockedBackgroundSkins);
        root.Child("Devices").Child(macAdress).Child("Save time").SetValueAsync(GetTime());
    }

    public static async Task<PlayerData> LoadDataOnlineAsync()
    {
        Debug.Log("Load data online initialized");
        
        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;
        PlayerData loadedData = new PlayerData();

        Task<DataSnapshot> nicknameData = root.Child("Devices").Child(macAdress).Child("Nickname").GetValueAsync();
        DataSnapshot nicknameSnap = await nicknameData;
        _nickname = nicknameSnap.Value.ToString();

        Task<DataSnapshot> scoresData = root.Child("Devices").Child(macAdress).Child("High score").GetValueAsync();
        DataSnapshot scoresSnap = await scoresData;
        loadedData.maxScores = Convert.ToInt32(scoresSnap.Value);

        Task<DataSnapshot> jumpsData = root.Child("Devices").Child(macAdress).Child("Total jumps").GetValueAsync();
        DataSnapshot jumpsSnap = await jumpsData;
        loadedData.totalJumps = Convert.ToInt32(jumpsSnap.Value);

        Task<DataSnapshot> currentCoinsData = root.Child("Devices").Child(macAdress).Child("Current coins").GetValueAsync();
        DataSnapshot currentCoinsSnap = await currentCoinsData;
        loadedData.currentCoins = Convert.ToInt32(currentCoinsSnap.Value);

        Task<DataSnapshot> coinsData = root.Child("Devices").Child(macAdress).Child("Total coins").GetValueAsync();
        DataSnapshot coinsSnap = await coinsData;
        loadedData.totalCoins = Convert.ToInt32(coinsSnap.Value);

        Task<DataSnapshot> playerSkinsData = root.Child("Devices").Child(macAdress).Child("Skins").Child("PlayerSkins").GetValueAsync();
        DataSnapshot playerSkinsSnap = await playerSkinsData;

        if(loadedData.unlockedPlayerSkins == null)
        {
            loadedData.unlockedPlayerSkins = new List<string>();
        }
        foreach(DataSnapshot miniSnap in playerSkinsSnap.Children)
        {
            loadedData.unlockedPlayerSkins.Add(miniSnap.Value.ToString());
        }

        Task<DataSnapshot> platformsSkinsData = root.Child("Devices").Child(macAdress).Child("Skins").Child("PlatformsSkins").GetValueAsync();
        DataSnapshot platformsSkinsSnap = await platformsSkinsData;

        if (loadedData.unlockedPlatformsSkins == null)
        {
            loadedData.unlockedPlatformsSkins = new List<string>();
        }
        foreach (DataSnapshot miniSnap in platformsSkinsSnap.Children)
        {
            loadedData.unlockedPlatformsSkins.Add(miniSnap.Value.ToString());
        }

        Task<DataSnapshot> backgroundSkinsData = root.Child("Devices").Child(macAdress).Child("Skins").Child("BackgroundSkins").GetValueAsync();
        DataSnapshot backgroundSkinsSnap = await backgroundSkinsData;

        if (loadedData.unlockedBackgroundSkins == null)
        {
            loadedData.unlockedBackgroundSkins = new List<string>();
        }
        foreach (DataSnapshot miniSnap in backgroundSkinsSnap.Children)
        {
            loadedData.unlockedBackgroundSkins.Add(miniSnap.Value.ToString());
        }

        Task<DataSnapshot> timeData = root.Child("Devices").Child(macAdress).Child("Save time").GetValueAsync();
        DataSnapshot timeSnap = await timeData;
        loadedData.saveTime = Convert.ToUInt64(timeSnap.Value);

        return loadedData;
    }
}

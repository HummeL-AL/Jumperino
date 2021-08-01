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
        DatabaseReference dir = FirebaseDatabase.DefaultInstance.RootReference.Child("Devices").Child(macAdress);

        dir.Child("Nickname").SetValueAsync(_nickname);
        dir.Child("High score").SetValueAsync(_maxScores);
        dir.Child("Total jumps").SetValueAsync(_totalJumps);
        dir.Child("Current coins").SetValueAsync(_currentCoins);
        dir.Child("Total coins").SetValueAsync(_totalCoins);
        dir.Child("Total ads watched").SetValueAsync(_totalAdsWatched);
        dir.Child("Total games played").SetValueAsync(_totalGamesPlayed);
        dir.Child("Skins").Child("PlayerSkins").SetValueAsync(_unlockedPlayerSkins);
        dir.Child("Skins").Child("PlatformsSkins").SetValueAsync(_unlockedPlatformsSkins);
        dir.Child("Skins").Child("BackgroundSkins").SetValueAsync(_unlockedBackgroundSkins);
        dir.Child("Save time").SetValueAsync(GetTime());
    }

    public static async Task<PlayerData> LoadDataOnlineAsync()
    {
        Debug.Log("Load data online initialized");
        
        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;
        PlayerData loadedData = new PlayerData();

        Task<DataSnapshot> allData = root.Child("Devices").Child(macAdress).GetValueAsync();
        DataSnapshot allSnap = await allData;

        _nickname = allSnap.Child("Nickname").Value.ToString();
        loadedData.maxScores = Convert.ToInt32(allSnap.Child("High score").Value);
        loadedData.totalJumps = Convert.ToInt32(allSnap.Child("Total jumps").Value);
        loadedData.currentCoins = Convert.ToInt32(allSnap.Child("Current coins").Value);
        loadedData.totalCoins = Convert.ToInt32(allSnap.Child("Total coins").Value);
        loadedData.totalAdsWatched = Convert.ToInt32(allSnap.Child("Total ads watched").Value);
        loadedData.totalGamesPlayed = Convert.ToInt32(allSnap.Child("Total games played").Value);

        if(loadedData.unlockedPlayerSkins == null)
        {
            loadedData.unlockedPlayerSkins = new List<string>();
        }
        foreach (DataSnapshot miniSnap in allSnap.Child("Skins").Child("PlayerSkins").Children)
        {
            loadedData.unlockedPlayerSkins.Add(miniSnap.Value.ToString());
        }

        if (loadedData.unlockedPlatformsSkins == null)
        {
            loadedData.unlockedPlatformsSkins = new List<string>();
        }
        foreach (DataSnapshot miniSnap in allSnap.Child("Skins").Child("PlatformsSkins").Children)
        {
            loadedData.unlockedPlatformsSkins.Add(miniSnap.Value.ToString());
        }

        if (loadedData.unlockedBackgroundSkins == null)
        {
            loadedData.unlockedBackgroundSkins = new List<string>();
        }
        foreach (DataSnapshot miniSnap in allSnap.Child("Skins").Child("BackgroundSkins").Children)
        {
            loadedData.unlockedBackgroundSkins.Add(miniSnap.Value.ToString());
        }

        loadedData.saveTime = Convert.ToUInt64(allSnap.Child("Save time").Value);

        return loadedData;
    }
}

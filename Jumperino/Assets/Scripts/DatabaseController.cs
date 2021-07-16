using System;
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
        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;

        root.Child("Devices").Child(macAdress).Child("High score").SetValueAsync(_maxScores);
        root.Child("Devices").Child(macAdress).Child("Total jumps").SetValueAsync(_totalJumps);
        root.Child("Devices").Child(macAdress).Child("Total coins").SetValueAsync(_totalCoins);
    }

    public static async Task<PlayerData> LoadDataOnlineAsync()
    {
        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;
        PlayerData loadedData = new PlayerData();

        Task<DataSnapshot> scoresData = root.Child("Devices").Child(macAdress).Child("High score").GetValueAsync();
        DataSnapshot scoresSnap = await scoresData;
        loadedData.maxScores = Convert.ToInt32(scoresSnap.Value);

        Task<DataSnapshot> jumpsData = root.Child("Devices").Child(macAdress).Child("Total jumps").GetValueAsync();
        DataSnapshot jumpsSnap = await jumpsData;
        loadedData.totalJumps = Convert.ToInt32(jumpsSnap.Value);

        Task<DataSnapshot> coinsData = root.Child("Devices").Child(macAdress).Child("Total coins").GetValueAsync();
        DataSnapshot coinsSnap = await coinsData;
        loadedData.totalCoins = Convert.ToInt32(coinsSnap.Value);

        return loadedData;
    }
}

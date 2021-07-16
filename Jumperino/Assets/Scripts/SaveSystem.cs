using System.IO;
using System.Net;
using UnityEngine;
using static PlayerData;
using static BinarySaver;
using static GameController;
using static DatabaseController;

public class SaveSystem : MonoBehaviour
{
    public static void TryToSaveData()
    {
        if(IsConnected())
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

    public static async void SyncData()
    {
        Debug.Log("Sync initialized");
        PlayerData offlineData = LoadDataOffline();
        PlayerData onlineData = await LoadDataOnlineAsync();

        if(offlineData.totalJumps > onlineData.totalJumps)
        {
            Debug.Log("Offline data newer: " + offlineData.maxScores + " " + offlineData.totalJumps + " " + offlineData.totalCoins);
            _maxScores = offlineData.maxScores;
            _totalJumps = offlineData.totalJumps;
            _totalCoins = offlineData.totalCoins;
            SaveDataOnline();
        }
        else
        {
            Debug.Log("Online data newer: " + onlineData.maxScores + " " + onlineData.totalJumps + " " + onlineData.totalCoins);
            _maxScores = onlineData.maxScores;
            _totalJumps = onlineData.totalJumps;
            _totalCoins = onlineData.totalCoins;
        }

        UpdateScores();
    }

    //public static PlayerData SaveDataOffline()
    //{
    //    Debug.Log("Save offline initialized");
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    string path = Application.persistentDataPath + "PlayerData.hmml";
    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    PlayerData data = new PlayerData();
    //    data.maxScores = _maxScores;
    //    data.totalJumps = _totalJumps;
    //    data.totalCoins = _totalCoins;

    //    formatter.Serialize(stream, data);
    //    stream.Close();

    //    return data;
    //}

    //public static PlayerData LoadDataOffline()
    //{
    //    Debug.Log("Load offline initialized");
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    string path = Application.persistentDataPath + "PlayerData.hmml";

    //    if (File.Exists(path))
    //    {
    //        FileStream stream = new FileStream(path, FileMode.Open);
    //        PlayerData data = formatter.Deserialize(stream) as PlayerData;
    //        stream.Close();

    //        _maxScores = data.maxScores;
    //        _totalJumps = data.totalJumps;
    //        _totalCoins = data.totalCoins;

    //        return data;
    //    }
    //    else
    //    {
    //        return SaveDataOffline();
    //    }
    //}

    //public static void SaveDataOnline()
    //{
    //    Debug.Log("Save online initialized");
    //    DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;

    //    root.Child("Devices").Child(macAdress).Child("High score").SetValueAsync(_maxScores);
    //    root.Child("Devices").Child(macAdress).Child("Total jumps").SetValueAsync(_totalJumps);
    //    root.Child("Devices").Child(macAdress).Child("Total coins").SetValueAsync(_totalCoins);
    //}

    //public static async Task<PlayerData> LoadDataOnlineAsync()
    //{
    //    Debug.Log("Load online initialized");
    //    DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;
    //    PlayerData loadedData = new PlayerData();

    //    Task<DataSnapshot> scoresData = root.Child("Devices").Child(macAdress).Child("High score").GetValueAsync();
    //    DataSnapshot scoresSnap = await scoresData;
    //    loadedData.maxScores = Convert.ToInt32(scoresSnap.Value);

    //    Task<DataSnapshot> jumpsData = root.Child("Devices").Child(macAdress).Child("Total jumps").GetValueAsync();
    //    DataSnapshot jumpsSnap = await jumpsData;
    //    loadedData.totalJumps = Convert.ToInt32(jumpsSnap.Value);

    //    Task<DataSnapshot> coinsData = root.Child("Devices").Child(macAdress).Child("Total coins").GetValueAsync();
    //    DataSnapshot coinsSnap = await coinsData;
    //    loadedData.totalCoins = Convert.ToInt32(coinsSnap.Value);

    //    return loadedData;
    //}

    public static string GetHtmlFromUrl(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    public static bool IsConnected()
    {
        string HtmlText = GetHtmlFromUrl("http://google.com");
        if (HtmlText == "")
        {
            Debug.Log("No internet connection!");
            return false;
        }
        else
        {
            if (!HtmlText.Contains("schema.org/WebPage"))
            {
                Debug.Log("Redirection!");
                return true;
            }
            else
            {
                Debug.Log("Internet connection detected.");
                return true;
            }
        }
    }
}

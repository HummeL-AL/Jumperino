using System;

[Serializable]
public class PlayerData
{
    public bool nicknameEntered;
    public int maxScores;
    public int totalJumps;
    public int currentCoins;
    public int totalCoins;

    public ulong saveTime;

    public static bool _nicknameEntered;
    public static string _nickname;
    public static int _maxScores;
    public static int _totalJumps;
    public static int _currentCoins;
    public static int _totalCoins;
}

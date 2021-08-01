using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public bool nicknameEntered;
    public int maxScores;
    public int totalJumps;
    public int currentCoins;
    public int totalCoins;
    public int totalAdsWatched;
    public int totalGamesPlayed;
    public List<string> unlockedPlayerSkins;
    public List<string> unlockedPlatformsSkins;
    public List<string> unlockedBackgroundSkins;

    public ulong saveTime;

    public static bool _nicknameEntered;
    public static string _nickname;
    public static int _maxScores;
    public static int _totalJumps;
    public static int _currentCoins;
    public static int _totalCoins;
    public static int _totalAdsWatched;
    public static int _totalGamesPlayed;
    public static List<string> _unlockedPlayerSkins;
    public static List<string> _unlockedPlatformsSkins;
    public static List<string> _unlockedBackgroundSkins;
}
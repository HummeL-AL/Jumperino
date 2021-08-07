using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using GoogleMobileAds.Api;
using static SaveSystem;
using static PlayerData;
using static GameController;

public class AdsManager : MonoBehaviour
{
    public GameObject ageConfirmer;

    public static int gamesToAd = 5;
    public static int numOfRetries = 5;

    static InterstitialAd interstitial;
    static RewardedAd rewarded;
    static List<String> testDeviceIds = new List<string>();

    public void Start()
    {
        TryToLoadData();

        if (_ageEntered)
        {
            ageConfirmer.GetComponent<AgeConfirm>().play.SetActive(true);
            ageConfirmer.SetActive(false);
            SetConfiguration();
        }

        testDeviceIds.Add("33BE2250B43518CCDA7DE426D04EE231");

    }

    public static void SetConfiguration()
    {
        RequestConfiguration configuration;
        if (_under13)
        {
            configuration = new RequestConfiguration.Builder().SetTestDeviceIds(testDeviceIds).SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True).SetMaxAdContentRating(MaxAdContentRating.G).build();
        }
        else
        {
            configuration = new RequestConfiguration.Builder().SetTestDeviceIds(testDeviceIds).build();
        }
        MobileAds.SetRequestConfiguration(configuration);
        MobileAds.Initialize(initStatus => { });

        RequestInterstitialAd();
        RequestRewardedAd();
    }

    public static void RequestInterstitialAd()
    {
        Debug.Log("Ad requested");

        string adUnitId = "ca-app-pub-5510419902180222/2073821073"; //"test: ca-app-pub-3940256099942544/1033173712";
        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);

        interstitial.OnAdLoaded += HandleOnInterstitialAdLoaded;
        interstitial.OnAdFailedToLoad += HandleOnInterstitialAdFailedToLoad;
        interstitial.OnAdOpening += HandleOnInterstitialAdOpened;
        interstitial.OnAdClosed += HandleOnInterstitialAdClosed;
    }

    public static void RequestRewardedAd()
    {
        string adUnitId = "ca-app-pub-5510419902180222/9162807753"; // "test: ca-app-pub-3940256099942544/5224354917";
        rewarded = new RewardedAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewarded.LoadAd(request);

        rewarded.OnAdLoaded += HandleOnRewardedAdLoaded;
        rewarded.OnAdFailedToLoad += GameContinueFailed;
        rewarded.OnAdOpening += HandleOnRewardedAdOpened;
        rewarded.OnAdClosed += HandleOnRewardedAdClosed;
        rewarded.OnUserEarnedReward += GameContinue;
    }

    public static void CheckInterstitialAd()
    {
        if (gamesToAd == 0)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
                gamesToAd = 5;
            }

            RequestInterstitialAd();
        }
    }

    public static void TryToReplayAd()
    {
        if (rewarded.IsLoaded())
        {
            rewarded.Show();
        }

        RequestInterstitialAd();
    }

    public static void CheckAdsCount()
    {
        if(_totalAdsWatched == 10)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 10));
        }
        else if (_totalAdsWatched == 20)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 20));
        }
        else if (_totalAdsWatched == 30)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 30));
        }
        else if (_totalAdsWatched == 40)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 40));
        }
        else if (_totalAdsWatched == 50)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 50));
        }
        else if (_totalAdsWatched == 75)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 75));
        }
        else if (_totalAdsWatched == 100)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 100));
        }
        else if (_totalAdsWatched == 150)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 150));
        }
        else if (_totalAdsWatched == 200)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 200));
        }
        else if (_totalAdsWatched == 250)
        {
            FirebaseAnalytics.LogEvent("Ads_Viewed", new Parameter("Count", 250));
        }
    }

    public static void CheckGamesCount()
    {
        if (_totalGamesPlayed == 5)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 5));
        }
        else if (_totalGamesPlayed == 10)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 10));
        }
        else if (_totalGamesPlayed == 25)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 25));
        }
        else if (_totalGamesPlayed == 50)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 50));
        }
        else if (_totalGamesPlayed == 75)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 75));
        }
        else if (_totalGamesPlayed == 100)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 100));
        }
        else if (_totalGamesPlayed == 150)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 150));
        }
        else if (_totalGamesPlayed == 200)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 200));
        }
        else if (_totalGamesPlayed == 250)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 250));
        }
        else if (_totalGamesPlayed == 300)
        {
            FirebaseAnalytics.LogEvent("Games_played", new Parameter("Count", 300));
        }
    }

    //Events area ---------------------------------------------------------------------------------------------

    public static void HandleOnInterstitialAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad loaded successful");
        numOfRetries = 5;
    }

    public static void HandleOnInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        if (numOfRetries == 0)
        {
            Debug.Log("Ad load failed. Error: " + args.LoadAdError);
        }
        else
        {
            Debug.Log("Ad load failed. Retry.");
            RequestInterstitialAd();
            numOfRetries--;
        }
    }

    public static void HandleOnInterstitialAdOpened(object sender, EventArgs args)
    {
        Debug.Log("Ad start showing");

        AudioListener.volume = 0f;
    }

    public static void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad closed");

        if (interstitial != null)
        {
            interstitial.Destroy();
            Debug.Log("Add destroyed");
        }

        AudioListener.volume = 1f;

        RequestInterstitialAd();
    }
    public static void HandleOnRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad loaded successful");
        numOfRetries = 5;
    }

    public static void HandleOnRewardedAdOpened(object sender, EventArgs args)
    {
        Debug.Log("Ad start showing");

        AudioListener.volume = 0f;
    }

    public static void HandleOnRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad closed");

        if (rewarded != null)
        {
            rewarded.Destroy();
            Debug.Log("Add Destroyed");
        }

        AudioListener.volume = 1f;

        RequestRewardedAd();
    }

    public static void GameContinue(object sender, Reward args)
    {
        Debug.Log("Game continue successful");
        FirebaseAnalytics.LogEvent("Total_Ads_Views", new Parameter("Type", "Game_Continue"));
        _totalAdsWatched++;

        CheckAdsCount();
        ReplayGame();
    }

    public static void GameContinueFailed(object sender, AdFailedToLoadEventArgs args)
    {
        if (numOfRetries == 0)
        {
            Debug.Log("Ad load failed. Error: " + args.LoadAdError);
            pc.game.RestartGame();
        }
        else
        {
            Debug.Log("Ad load failed. Retry.");
            RequestInterstitialAd();
            numOfRetries--;
        }
    }

    //---------------------------------------------------------------------------------------------------------
}

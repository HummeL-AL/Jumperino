using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerData;
using static SaveSystem;
using static AdsManager;

public class AgeConfirm : MonoBehaviour
{
    public GameObject play;

    public void SetAgeUnder13()
    {
        _under13 = true;
        _ageEntered = true;
        SetConfiguration();

        TryToSaveData();

        play.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetAgeOver13()
    {
        _under13 = false;
        _ageEntered = true;
        SetConfiguration();

        TryToSaveData();

        play.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("https://hummel777.github.io/Jumperino/");
    }
}

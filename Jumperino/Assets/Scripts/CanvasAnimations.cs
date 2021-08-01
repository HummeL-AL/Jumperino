using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimations : MonoBehaviour
{
    public GameObject Name;
    public GameObject ButtonPanel;
    public GameObject RestartGame;
    public GameObject ShopPanel;
    public GameObject RankPanel;
    public GameObject SettingsPanel;
    public GameObject CoinsPanel;
    public GameObject Scores;

    public Scoreboard scoreboard;

    public void Start()
    {
        RestartGame.SetActive(false);
        ShopPanel.SetActive(false);
        RankPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        Scores.SetActive(false);
    }

    public void MenuEnable()
    {
        Name.SetActive(true);
        ButtonPanel.SetActive(true);
        Scores.SetActive(false);
    }

    public void MenuDisable()
    {
        Name.SetActive(false);
        ButtonPanel.SetActive(false);
        Scores.SetActive(true);
    }

    //public void MenuSwap()
    //{
    //    Name.SetActive(!Name.activeSelf);
    //    ButtonPanel.SetActive(!ButtonPanel.activeSelf);
    //    Scores.SetActive(!Scores.activeSelf);
    //}

    public void RestartPanelSwap()
    {
        RestartGame.SetActive(!RestartGame.activeSelf);
    }

    public void ShopPanelSwap()
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf);
    }

    public void RankPanelSwap()
    {
        RankPanel.SetActive(!RankPanel.activeSelf);
        if(RankPanel.activeSelf)
        {
            RankPanel.GetComponent<Scoreboard>().InitializeScoreboard();
        }
    }

    public void SettingsPanelSwap()
    {
        SettingsPanel.SetActive(!SettingsPanel.activeSelf);
    }
}

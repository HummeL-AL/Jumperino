using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerData;
using static SkinLoader;
using static SaveSystem;
using static GameController;

public class Shop : MonoBehaviour
{
    public GameObject shopItem;

    public Transform playerSkinPanel;
    public Transform platformSkinPanel;
    public Transform backgroundSkinPanel;
    public Transform coinSkinPanel;

    private void Awake()
    {
        playerSkins = (PlayerSkin[])SortSkins(playerSkins);
        platformSkins = (PlatformSkin[])SortSkins(platformSkins);
        backgroundSkins = (BackgroundSkin[])SortSkins(backgroundSkins);
        coinSkins = (CoinSkin[])SortSkins(coinSkins);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Skin[] SortSkins(Skin[] skinArray)
    {
        Skin[] sortedArray = skinArray;

        for(int i = 1; i < sortedArray.Length; i++)
        {
            Skin temp = sortedArray[i];
            for (int j = i - 1; j >= 0; j--)
            {
                if (temp.price < sortedArray[j].price)
                {
                    sortedArray[j + 1] = sortedArray[j];

                    if(j==0)
                    {
                        sortedArray[j] = temp;
                    }
                }
                else
                {
                    sortedArray[j + 1] = temp;
                    break;
                }
            }
        }

        return sortedArray;
    }

    public void UpdateShop()
    {
        foreach(Transform panel in playerSkinPanel)
        {
            Destroy(panel.gameObject);
        }
        foreach (Transform panel in platformSkinPanel)
        {
            Destroy(panel.gameObject);
        }
        foreach (Transform panel in backgroundSkinPanel)
        {
            Destroy(panel.gameObject);
        }
        foreach (Transform panel in coinSkinPanel)
        {
            Destroy(panel.gameObject);
        }

        foreach (PlayerSkin skin in playerSkins)
        {
            Transform createdPanel = Instantiate(shopItem, playerSkinPanel.transform).transform;
            createdPanel.name = skin.skinName;
            createdPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Skins/Player/" + skin.skinName + "/Preview");

            createdPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skin.price.ToString();

            if (_unlockedPlayerSkins == null)
            {
                _unlockedPlayerSkins = new List<string>();
            }

            if (_unlockedPlayerSkins.Contains(skin.skinName))
            {
                createdPanel.GetChild(1).gameObject.SetActive(false);
                createdPanel.GetChild(2).gameObject.SetActive(true);
            }
        }

        foreach (PlatformSkin skin in platformSkins)
        {
            Transform createdPanel = Instantiate(shopItem, platformSkinPanel.transform).transform;
            createdPanel.name = skin.skinName;
            createdPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Skins/Platforms/" + skin.skinName + "/" + skin.skinName);

            createdPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skin.price.ToString();

            if (_unlockedPlatformsSkins == null)
            {
                _unlockedPlatformsSkins = new List<string>();
            }

            if (_unlockedPlatformsSkins.Contains(skin.skinName))
            {
                createdPanel.GetChild(1).gameObject.SetActive(false);
                createdPanel.GetChild(2).gameObject.SetActive(true);
            }
        }

        foreach (BackgroundSkin skin in backgroundSkins)
        {
            Transform createdPanel = Instantiate(shopItem, backgroundSkinPanel.transform).transform;
            createdPanel.name = skin.skinName;
            createdPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Skins/Background/" + skin.skinName + "/Preview");

            createdPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skin.price.ToString();

            if (_unlockedBackgroundSkins == null)
            {
                _unlockedBackgroundSkins = new List<string>();
            }

            if (_unlockedBackgroundSkins.Contains(skin.skinName))
            {
                createdPanel.GetChild(1).gameObject.SetActive(false);
                createdPanel.GetChild(2).gameObject.SetActive(true);
            }
        }

        foreach (CoinSkin skin in coinSkins)
        {
            Transform createdPanel = Instantiate(shopItem, coinSkinPanel.transform).transform;
            createdPanel.name = skin.skinName;
            createdPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Skins/Coins/" + skin.skinName + "/" + skin.skinName);

            createdPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skin.price.ToString();

            if (_unlockedCoinsSkins == null)
            {
                _unlockedCoinsSkins = new List<string>();
            }

            if (_unlockedCoinsSkins.Contains(skin.skinName))
            {
                createdPanel.GetChild(1).gameObject.SetActive(false);
                createdPanel.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void TryToBuySkin(Transform skinToBuy)
    {
        int skinPrice = Convert.ToInt32(skinToBuy.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        string skinName = skinToBuy.name;

        if (_currentCoins >= skinPrice)
        {
            _currentCoins -= skinPrice;

            switch(skinToBuy.parent.name)
            {
                case "Player":
                    {
                        if (_unlockedPlayerSkins == null)
                        {
                            _unlockedPlayerSkins = new List<string>();
                        }
                        _unlockedPlayerSkins.Add(skinName);
                        break;
                    }
                case "Platforms":
                    {
                        if (_unlockedPlatformsSkins == null)
                        {
                            _unlockedPlatformsSkins = new List<string>();
                        }
                        _unlockedPlatformsSkins.Add(skinName);
                        break;
                    }
                case "Background":
                    {
                        if (_unlockedBackgroundSkins == null)
                        {
                            _unlockedBackgroundSkins = new List<string>();
                        }
                        _unlockedBackgroundSkins.Add(skinName);
                        break;
                    }
                case "Coins":
                    {
                        if (_unlockedCoinsSkins == null)
                        {
                            _unlockedCoinsSkins = new List<string>();
                        }
                        _unlockedCoinsSkins.Add(skinName);
                        break;
                    }
            }

            skinToBuy.GetChild(1).gameObject.SetActive(false);
            skinToBuy.GetChild(2).gameObject.SetActive(true);

            TryToSaveData();
            TryToSaveSettings();
            TryToApplySkin(skinToBuy);
        }
    }

    public void TryToApplySkin(Transform skinToApply)
    {
        string skinName = skinToApply.name;

        switch (skinToApply.parent.name)
        {
            case "Player":
                {
                    foreach (PlayerSkin skin in playerSkins)
                    {
                        if (skin.skinName == skinName)
                        {
                            pc.Skin = skin;
                            break;
                        }
                    }
                    break;
                }
            case "Platforms":
                {
                    foreach (PlatformSkin skin in platformSkins)
                    {
                        if (skin.skinName == skinName)
                        {
                            Spawner._platformSkin = skin;
                            break;
                        }
                    }
                    break;
                }
            case "Background":
                {
                    foreach (BackgroundSkin skin in backgroundSkins)
                    {
                        if (skin.skinName == skinName)
                        {
                            cam.GetComponent<Global>().Skin = skin;
                            break;
                        }
                    }
                    break;
                }
            case "Coins":
                {
                    foreach (CoinSkin skin in coinSkins)
                    {
                        if (skin.skinName == skinName)
                        {
                            Coin.Skin = skin;
                            break;
                        }
                    }
                    break;
                }
        }

        TryToSaveSettings();
    }
}

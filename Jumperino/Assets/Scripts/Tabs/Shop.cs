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

    Transform playerSkinPanel;
    Transform platformSkinPanel;
    Transform backgroundSkinPanel;

    private void Awake()
    {
        playerSkinPanel = transform.GetChild(0).GetChild(0).GetChild(0);
        platformSkinPanel = transform.GetChild(1).GetChild(0).GetChild(0);
        backgroundSkinPanel = transform.GetChild(2).GetChild(0).GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            createdPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Skins/Platforms/" + skin.skinName + "/Preview");

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
    }

    public void TryToBuySkin(Transform skinToBuy)
    {
        int skinPrice = Convert.ToInt32(skinToBuy.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        string skinName = skinToBuy.name;

        if (_currentCoins > skinPrice)
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
            }

            skinToBuy.GetChild(1).gameObject.SetActive(false);
            skinToBuy.GetChild(2).gameObject.SetActive(true);

            TryToSaveData();
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
        }
    }
}
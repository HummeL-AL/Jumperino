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

        foreach(PlayerSkin skin in playerSkins)
        {
            Transform createdPanel = Instantiate(shopItem, playerSkinPanel.transform).transform;
            createdPanel.name = skin.skinName;
            createdPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Skins/" + skin.skinName + "/Preview");

            Debug.Log(_unlockedPlayerSkins);
            Debug.Log(skin.skinName);

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
    }

    public void TryToBuySkin(Transform skinToBuy)
    {
        int skinPrice = Convert.ToInt32(skinToBuy.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        string skinName = skinToBuy.name;

        if (_currentCoins > skinPrice)
        {
            _currentCoins -= skinPrice;

            if (_unlockedPlayerSkins == null)
            {
                _unlockedPlayerSkins = new List<string>();
            }
            _unlockedPlayerSkins.Add(skinName);

            skinToBuy.GetChild(1).gameObject.SetActive(false);
            skinToBuy.GetChild(2).gameObject.SetActive(true);

            TryToSaveData();
        }
    }

    public void TryToApplySkin(Transform skinToBuy)
    {
        string skinName = skinToBuy.name;

        foreach(PlayerSkin skin in playerSkins)
        {
            if(skin.skinName == skinName)
            {
                pc.skin = skin;
                break;
            }
        }
    }
}

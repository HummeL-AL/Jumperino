using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;
using static PlayerData;
using static GameController;

public class Coin : MonoBehaviour
{
    public bool playerTouched = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        if (playerTouched)
        {
            CollectCoin();
        }
    }

    public void CollectCoin()
    {
        AudioSource.PlayClipAtPoint(pc.coinPickupSound, transform.position, soundVolume);
        curCoins++;
        _currentCoins++;
        _totalCoins++;
        UpdateScores();
    }
}

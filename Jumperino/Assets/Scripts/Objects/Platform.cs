using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spawner;

public class Platform : MonoBehaviour
{
    public bool first;
    public bool activated;

    public bool withCoin;
    public coinType coinStructure;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        spawnCoins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {

    }

    public void spawnCoins()
    {
        if (withCoin)
        {
            switch (coinStructure)
            {
                case coinType.low:
                    {
                        Instantiate(_coinPrefab, transform.position + Vector3.up * 1f, Quaternion.identity, transform);
                        break;
                    }
                case coinType.medium:
                    {
                        Instantiate(_coinPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform);
                        break;
                    }
                case coinType.high:
                    {
                        Instantiate(_coinPrefab, transform.position + Vector3.up * 3f, Quaternion.identity, transform);
                        break;
                    }
            }
        }
    }
}

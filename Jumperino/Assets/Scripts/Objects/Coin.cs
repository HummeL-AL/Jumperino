using UnityEngine;
using static Global;
using static PlayerData;
using static GameController;

public class Coin : MonoBehaviour
{
    public bool playerTouched = false;

    public static CoinSkin skin;
    public static CoinSkin Skin
    {
        get => skin;
        set
        {
            skin = value;
            Texture2D tex = skin.sprite.texture;
            Texture2D targetTex = _coinSkin.texture;

            for (int i = 0; i < targetTex.height; i++)
            {
                for (int j = 0; j < targetTex.width; j++)
                {
                    targetTex.SetPixel(j, i, Color.clear);
                    int xOffset = (targetTex.width - tex.width) / 2;
                    int yOffset = (targetTex.height - tex.height) / 2;

                    int offsettedX = j - xOffset;
                    int offsettedY = i - yOffset;
                    if (offsettedX > 0 && offsettedX < tex.width && offsettedY > 0 && offsettedY < tex.height)
                    {
                        targetTex.SetPixel(j, i, tex.GetPixel(offsettedX, offsettedY));

                    }
                }
            }

            targetTex.Apply();
        }
    }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = skin.sprite;
        GetComponent<BoxCollider2D>().size = skin.colliderSize;
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

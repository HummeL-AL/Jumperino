using UnityEngine;

public class SkinLoader
{
    public static PlayerSkin[] playerSkins = Resources.LoadAll<PlayerSkin>("Skins/Player");
    public static PlatformSkin[] platformSkins = Resources.LoadAll<PlatformSkin>("Skins/Platforms");
    public static BackgroundSkin[] backgroundSkins = Resources.LoadAll<BackgroundSkin>("Skins/Background");
    public static CoinSkin[] coinSkins = Resources.LoadAll<CoinSkin>("Skins/Coins");
}

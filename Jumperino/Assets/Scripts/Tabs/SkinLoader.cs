using UnityEngine;

public class SkinLoader
{
    public static PlayerSkin[] playerSkins = Resources.LoadAll<PlayerSkin>("Skins/Player");
    public static PlatformSkin[] platformSkins;
    public static BackgroundSkin[] backgroundSkins;
}

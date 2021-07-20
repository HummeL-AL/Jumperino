using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewSkin", menuName = "Skin/Platform")]
public class PlatformSkin : ScriptableObject
{
    public string skinName;
    public int price;

    public Sprite sprite;
    public Vector2 colliderSize;
}

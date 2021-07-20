using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewSkin", menuName = "Skin/Background")]
public class BackgroundSkin : ScriptableObject
{
    public string skinName;
    public int price;

    public Mesh mesh;
    public Material[] materials;
}

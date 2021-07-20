using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewSkin", menuName = "Skin/Player")]
public class PlayerSkin : ScriptableObject
{
    public string skinName;
    public int price;

    public Mesh mesh;
    public Material[] materials;
}

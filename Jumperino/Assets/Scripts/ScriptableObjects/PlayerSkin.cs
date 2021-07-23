using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu (fileName = "NewSkin", menuName = "Skin/Player")]
public class PlayerSkin : ScriptableObject
{
    public string skinName;
    public int price;

    public Mesh mesh;
    public Material[] materials;
    public ParticleSystem landParticles;
}

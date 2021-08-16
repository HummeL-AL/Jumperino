using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu (fileName = "NewSkin", menuName = "Skin/Player")]
public class PlayerSkin : Skin
{
    public Mesh mesh;
    public Material[] materials;
    public ParticleSystem landParticles;
    public AudioClip landSound;
}

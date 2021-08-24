using UnityEngine;

[CreateAssetMenu (fileName = "NewSkin", menuName = "Skin/Background")]
public class BackgroundSkin : Skin
{
    public Material material;
    public ParticleSystem ambientParticles;
    public AudioClip ambient;
}

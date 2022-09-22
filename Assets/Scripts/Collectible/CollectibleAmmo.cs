using UnityEngine;

public class CollectibleAmmo : Collectible
{
    [SerializeField] ParticleSystem m_collectFXPrefab;

    public override void ApplyEffect(Player player)
    {
        player.weaponController.IncreaseMagazineSize();
        Instantiate(m_collectFXPrefab, transform.position, Quaternion.identity);
        AudioManager.i.PlaySFX("reload");

        Disappear();
    }
}
using UnityEngine;

public class CollectibleHealth : Collectible
{
    private int healthPoint = 100;
    [SerializeField] ParticleSystem m_collectFXPrefab;

    public override void ApplyEffect(Player player)
    {
        player.Heal(healthPoint);
        Instantiate(m_collectFXPrefab, transform.position, Quaternion.identity);
        AudioManager.i.PlaySFX("heal");
        
        Disappear();
    }
}
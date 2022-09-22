using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float m_health = 100f;
    public float health => m_health;

    [SerializeField] protected float m_maxHealth = 100f;
    public float maxHealth => m_maxHealth;

    public virtual void ApplyDamage(int damage)
    {
        m_health -= damage;

        if(m_health <= 0)
        {
            m_health = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
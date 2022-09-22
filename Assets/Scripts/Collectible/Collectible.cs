using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Collectible : MonoBehaviour
{
    public IObjectPool<Collectible> Pool { get; set; }

    [SerializeField] protected float m_lifetime = 3f;
    [SerializeField] protected float m_rotationSpeed = 60f;

    protected virtual void Update()
    {
        transform.Rotate (Vector3.up * m_rotationSpeed * Time.deltaTime, Space.World);
    }

    public virtual void Init()
    {
        StartCoroutine(DisappearAfterLifetime());
    }

    protected virtual IEnumerator DisappearAfterLifetime()
    {
        yield return new WaitForSeconds(m_lifetime);

        Disappear();
    }

    public virtual void Disappear()
    {
        if(Pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Pool.Release(this);
        }
    }
    
    public abstract void ApplyEffect(Player player);
}
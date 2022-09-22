using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : Entity
{
    private Vector2 m_direction;

    public IObjectPool<Enemy> Pool { get; set; }

    private NavMeshAgent m_agent;
    public NavMeshAgent agent => m_agent;

    private Animator m_animator;
    public Animator animator => m_animator;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponentInChildren<Animator>();
    }

    private void Start() 
    {
        m_agent.updateRotation = false;
        LevelManager.i.onLevelEnded.AddListener(m_agent.ResetPath);
    }

    private void Update() 
    {
        m_direction = new Vector2(m_agent.velocity.x, m_agent.velocity.z);
        m_animator.SetBool("isMove", m_direction.magnitude > 0f);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("PlayerWeapon"))
        {
            ApplyDamage(other.GetComponent<Weapon>().damage);
        }
    }

    public void OnTakeFromPool()
    {
        if(Pool == null) return;

        m_health = m_maxHealth;
    }

    public override void Die()
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
}

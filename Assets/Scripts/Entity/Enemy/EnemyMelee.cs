using UnityEngine;

[RequireComponent(typeof(EnemyMeleeFSM))]
public class EnemyMelee : MonoBehaviour 
{
    [SerializeField] private Enemy m_main;
    public Enemy main => m_main;

    [SerializeField] private int m_damage = 10;
    public int damage => m_damage;

    [SerializeField] private float m_sightRange = 5f;
    public float sightRange => m_sightRange;

    [SerializeField] private float m_attackRange = 4f;
    public float attackRange => m_attackRange;
    
    [SerializeField] private LayerMask m_whatIsPlayer;

    [SerializeField] private float m_attackSpeed = 2f;
    public float attackSpeed => m_attackSpeed;
    
    [SerializeField, Range(0f, 1f)] private float m_strafeDistance = 5f;
    public float strafeDistance => m_strafeDistance;

    [SerializeField, Range(0f, 1f)] private float m_rotationSmooth = .3f;

    [SerializeField] private Sword m_sword;
    public Sword sword => m_sword;

    public EnemyMeleeFSM FSM { get; set; }

    private void Awake()
    {
        FSM = GetComponent<EnemyMeleeFSM>();
    }

    private void Start() 
    {
        m_sword.Init(m_damage);
    }

    public void LookAt(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSmooth);
    }

    public void LookAtPlayer()
    {
        LookAt(LevelManager.i.player.transform.position - transform.position);
    }

    public bool IsPlayerInSight()
    {
        return Physics.CheckSphere(transform.position, m_sightRange, m_whatIsPlayer);
    }

    public bool IsPlayerInAttackRange()
    {
        return Physics.CheckSphere(transform.position, m_attackRange, m_whatIsPlayer);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(transform.position, m_sightRange);

        Gizmos.color = Color.red;    
        Gizmos.DrawWireSphere(transform.position, m_attackRange);   
    }
}
using UnityEngine;
using UnityEngine.AI;
using RobustFSM.Base;

[RequireComponent(typeof(EnemyRangeFSM))]
public class EnemyRange : MonoBehaviour
{
    [SerializeField] private Enemy m_main;
    public Enemy main => m_main;

    [SerializeField] private int m_damage = 10;
    public int damage => m_damage;

    [SerializeField] private float m_sightRange = 5f;
    public float sightRange => m_sightRange;

    [SerializeField] private float m_shootRange = 4f;
    public float shootRange => m_shootRange;

    [SerializeField] private LayerMask m_whatIsPlayer;

    [SerializeField] private float m_shootSpeed = 5f;
    public float shootSpeed => m_shootSpeed;

    [SerializeField] private float m_timeBetweenShoot = 0f;
    public float timeBetweenShoot => m_timeBetweenShoot;
    
    [SerializeField] private Transform m_shootPoint;
    public Transform shootPoint => m_shootPoint;
    
    [SerializeField] private Bullet m_bulletPrefab;
    public Bullet bulletPrefab => m_bulletPrefab;

    [SerializeField, Range(0f, 1f)] private float m_rotationSmooth = .3f;

    public void LookAtPlayer()
    {
        LookAt(LevelManager.i.player.transform.position - transform.position);
    }

    public void LookAt(Vector3 direction)
    {
        if(direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSmooth);
    }

    public bool IsPlayerInSight()
    {
        return Physics.CheckSphere(transform.position, m_sightRange, m_whatIsPlayer);
    }

    public bool IsPlayerInShootRange()
    {
        return Physics.CheckSphere(transform.position, m_shootRange, m_whatIsPlayer);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(transform.position, m_sightRange);

        Gizmos.color = Color.red;    
        Gizmos.DrawWireSphere(transform.position, m_shootRange);    
    }
}

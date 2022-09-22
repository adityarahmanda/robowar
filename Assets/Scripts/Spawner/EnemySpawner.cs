using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Object Pool Settings")]
    [SerializeField] private bool m_collectionChecks = true;
    [SerializeField] private int m_maxPoolSize = 30;

    [Header("Spawner Settings")]
    [SerializeField] private Transform m_spawnCenter;
    [SerializeField] private float m_minSpawnTime = 5f;
    [SerializeField] private float m_maxSpawnTime = 10f;
    private float m_spawnTimeCounter;

    [SerializeField] private float m_minSpawnRange = 10f;
    [SerializeField] private float m_maxSpawnRange = 15f;

    private float m_groundHeightOffset = 1.5f;

    [SerializeField] private SpawnItem[] m_spawnItems;

    private IObjectPool<Enemy> m_Pool;
    public IObjectPool<Enemy> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<Enemy>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_collectionChecks, 10, m_maxPoolSize);
            }
            return m_Pool;
        }
    }

    private void Start() 
    {
        // spawn 3 enemies on start
        for(int i = 0; i < 3; i++)
        {
            Spawn();
        }

        m_spawnTimeCounter = Random.Range(m_minSpawnTime, m_maxSpawnTime);
    }

    private void Update() 
    {
        if(LevelManager.i.LevelEnded) return;
        
        if(m_spawnTimeCounter > 0f)
        {
            m_spawnTimeCounter -= Time.deltaTime;
        }
        else
        {
            Spawn();
            m_spawnTimeCounter = Random.Range(m_minSpawnTime, m_maxSpawnTime);
        }
    }

    public void Spawn()
    {
        int xSide = Random.Range(0, 1f) > .5f ? 1 : -1;
        int zSide = Random.Range(0, 1f) > .5f ? 1 : -1;

        Vector3 randomPos = new Vector3(Random.Range(m_minSpawnRange * xSide, m_maxSpawnRange * xSide), 0, Random.Range(m_minSpawnRange * zSide, m_maxSpawnRange * zSide));

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPos, out hit, 1f, NavMesh.AllAreas) && hit.position.y <= m_groundHeightOffset)
        {
            Enemy enemy = Pool.Get();
            enemy.agent.Warp(hit.position);
        }
        else
        {
            Spawn();
        }
    }

    private Enemy CreatePooledItem()
    {
        float random = Random.Range(0f, 1f);
        float total = 0;
        float numForAdding = 0;

        for(int i  = 0; i < m_spawnItems.Length; i++)
        {
            total += m_spawnItems[i].percentage;
        }

        for(int i  = 0; i < m_spawnItems.Length; i++)
        {
            if(m_spawnItems[i].percentage / total + numForAdding >= random)
            {
                Enemy enemy = Instantiate(m_spawnItems[i].prefab).GetComponent<Enemy>();
                enemy.Pool = Pool;

                return enemy;
            }
            else
            {
                numForAdding += m_spawnItems[i].percentage / total;
            }
        }

        return null;
    }

    private void OnReturnedToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.OnTakeFromPool();
    }

    private void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(m_spawnCenter.position, m_minSpawnRange);

        Gizmos.color = Color.red;    
        Gizmos.DrawWireSphere(m_spawnCenter.position, m_maxSpawnRange);
    }
}
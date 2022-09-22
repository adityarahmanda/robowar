using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Object Pool Settings")]
    [SerializeField] private bool m_collectionChecks = true;
    [SerializeField] private int m_maxPoolSize = 30;

    [Header("Spawner Settings")]
    [SerializeField] private float m_minSpawnTime = 5f;
    [SerializeField] private float m_maxSpawnTime = 10f;
    private float m_spawnTimeCounter;

    [SerializeField] private float m_spawnRange = 10f;

    private float m_groundHeightOffset = 1.5f;

    [SerializeField] private SpawnItem[] m_spawnItems;

    private IObjectPool<Collectible> m_Pool;
    public IObjectPool<Collectible> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<Collectible>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_collectionChecks, 10, m_maxPoolSize);
            }
            return m_Pool;
        }
    }

    private void Start() 
    {
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
        Vector3 randomPos = transform.position + Random.insideUnitSphere * m_spawnRange;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPos, out hit, 1f, NavMesh.AllAreas) && hit.position.y <= m_groundHeightOffset)
        {
            Collectible collectible = Pool.Get();
            collectible.transform.position = new Vector3(hit.position.x, 1.25f, hit.position.z);
        }
        else
        {
            Spawn();
        }
    }

    private Collectible CreatePooledItem()
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
                Collectible collectible = Instantiate(m_spawnItems[i].prefab, transform.position, m_spawnItems[i].prefab.transform.rotation).GetComponent<Collectible>();
                collectible.Pool = Pool;

                return collectible;
            }
            else
            {
                numForAdding += m_spawnItems[i].percentage / total;
            }
        }

        return null;
    }

    private void OnReturnedToPool(Collectible collectible)
    {
        collectible.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Collectible collectible)
    {
        collectible.gameObject.SetActive(true);
        collectible.Init();
    }

    private void OnDestroyPoolObject(Collectible collectible)
    {
        Destroy(collectible.gameObject);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(transform.position, m_spawnRange);
    }
}
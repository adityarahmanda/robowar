using UnityEngine;

public class Destructible : MonoBehaviour 
{
    private Material m_material;

    [SerializeField] private bool m_isStartDissolve = false;
    [SerializeField] private float m_dissolveAmount = 0;
    [SerializeField] private float m_dissolveTime = 1f;

    private void Start() 
    {
        m_material = GetComponent<Renderer>().material;
    }

    private void Update() 
    {
        if(m_isStartDissolve)
        {
            m_dissolveAmount += Time.deltaTime / m_dissolveTime;

            m_material.SetFloat("_DissolveAmount", m_dissolveAmount);

            if(m_dissolveAmount >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("PlayerWeapon") || other.gameObject.CompareTag("EnemyWeapon"))
        {
            m_isStartDissolve = true;
        }    
    }
}
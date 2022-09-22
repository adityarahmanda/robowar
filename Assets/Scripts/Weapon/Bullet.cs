using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    private float m_range;
    private float m_distanceTravelled; 

    private Vector3 m_initPos;
    private Rigidbody m_body;

    private void Awake() 
    {
        m_body = GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        m_initPos = transform.position;
    }

    private void Update() 
    {
        m_distanceTravelled = Vector3.Distance(transform.position, m_initPos);

        if(m_distanceTravelled > m_range)
        {
            Destroy(gameObject);
        }
    }

    public void Init(int damage, float range, Vector3 direction, float force) 
    {
        base.Init(damage);
        m_range = range;
        transform.forward = direction;
        m_body.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) 
    {
        Destroy(gameObject); 
    }
}
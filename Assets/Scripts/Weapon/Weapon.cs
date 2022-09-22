using UnityEngine;

public class Weapon : MonoBehaviour 
{
    protected int m_damage;
    public int damage 
    {
        get { return m_damage; }
    }

    public virtual void Init(int damage)
    {
        m_damage = damage;
    }
}
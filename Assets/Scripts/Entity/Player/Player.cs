using UnityEngine;

public class Player : Entity 
{
    private Rigidbody m_body;
    public Rigidbody body => m_body;

    private CharacterController m_characterController;
    public CharacterController characterController => m_characterController;

    private PlayerMoveController m_moveController;
    public PlayerMoveController moveController => m_moveController;

    private PlayerWeaponController m_weaponController;
    public PlayerWeaponController weaponController => m_weaponController;

    private PlayerWeaponRangeIndicator m_weaponRangeIndicator;
    public PlayerWeaponRangeIndicator weaponRangeIndicator => m_weaponRangeIndicator;

    private Animator m_animator;
    public Animator animator => m_animator;

    private bool m_isDie;
    public bool isDie => m_isDie;

    private void Awake() 
    {
        m_body = GetComponentInChildren<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
        m_characterController = GetComponentInChildren<CharacterController>();
        m_moveController = GetComponentInChildren<PlayerMoveController>();
        m_weaponController = GetComponentInChildren<PlayerWeaponController>();
        m_weaponRangeIndicator = GetComponentInChildren<PlayerWeaponRangeIndicator>();
    }

    public void Heal(int healthPoint)
    {
        m_health += healthPoint;

        if(m_health > m_maxHealth)
        {
            m_health = m_maxHealth;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("EnemyWeapon"))
        {
            ApplyDamage(other.GetComponent<Weapon>().damage);
        }

        if(other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.GetComponent<Collectible>().ApplyEffect(this);
        }
    }

    public override void Die()
    {
        m_isDie = true;
        m_animator.SetTrigger("die");
        LevelManager.i.EndLevel();
    }
}
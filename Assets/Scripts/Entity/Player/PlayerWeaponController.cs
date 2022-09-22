using System.Collections;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour 
{
    [SerializeField] private Player m_main;
    public Player main => m_main;

    private int m_damage = 15;

    [Range(0, 30f)] private float m_range = 5f;
    public float range => m_range;

    [SerializeField, Range(0, 5)] private int m_magazineSize = 3;
    public int magazineSize => m_magazineSize;

    [SerializeField, Range(0, 5)] private int m_maxMagazineSize = 5;

    [SerializeField] private float m_bulletsLeft;
    public float bulletsLeft => m_bulletsLeft;

    [SerializeField, Range(0, 10f)] private float m_bulletsCooldownTime = .3f;

    [SerializeField, Range(0, 1f)] private float m_shootBuffer = .3f;
    private float m_shootBufferCounter;
    [SerializeField] private float m_shootDuration = .3f;
    [SerializeField] private float m_shootSpeed = 1f;
    private float m_shootForce = 3f;
    private float m_rotationOffset = .25f;

    [SerializeField] private Bullet m_bulletPrefab;
    [SerializeField] private Transform m_shootPoint;

    private Vector2 m_attackInput;
    private Vector3 m_shootingDirection;
    public Vector3 shootingDirection => m_shootingDirection;

    private Animator m_animator;
    private PlayerMoveController m_moveController;

    private bool m_isShooting;
    public bool IsShooting => m_isShooting;
    
    private void Start()
    {
        m_animator = m_main.animator;
        m_moveController = m_main.moveController;
        
        m_bulletsLeft = m_magazineSize;

        InputManager.i.AttackInput.onDrag.AddListener(OnDragAttackInput);
        InputManager.i.AttackInput.onPointerUp.AddListener(OnPointerUpAttackInput);
    }

    private void Update() 
    {
        if(main.isDie) return;

        m_shootBufferCounter -= Time.deltaTime;

        if(!m_isShooting && m_bulletsLeft >= 1f && m_shootBufferCounter > 0f)
        {
            StartShoot();
            m_shootBufferCounter = 0;
        }

        Reload();
    }

    private void StartShoot()
    {
        m_isShooting = true;
        m_bulletsLeft--;

        StartCoroutine(PrepareShoot());
    }

    private IEnumerator PrepareShoot()
    {
        m_shootingDirection = new Vector3(m_attackInput.x, 0, m_attackInput.y);
        
        // look to shoot direction
        Quaternion targetRotation = Quaternion.LookRotation(m_shootingDirection, Vector3.up); 
        yield return new WaitUntil(() => m_moveController.root.rotation.eulerAngles.y >= targetRotation.eulerAngles.y - m_rotationOffset && m_moveController.root.rotation.eulerAngles.y <= targetRotation.eulerAngles.y + m_rotationOffset);

        // start shooting
        m_animator.SetFloat("shootSpeed", m_shootSpeed);
        m_animator.SetBool("isShoot", true);

        // end shoot on timeout
        Invoke("EndShoot", m_shootDuration);
    }

    public void Shoot()
    {
        if(m_bulletPrefab != null)
        {
            Bullet currentBullet = Instantiate(m_bulletPrefab, m_shootPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet        
            currentBullet.transform.forward = m_shootingDirection.normalized;
            
            float bulletRange =  m_range - Vector3.Distance(m_main.transform.position, m_shootPoint.transform.position);
            currentBullet.Init(m_damage, bulletRange, m_shootingDirection.normalized, m_shootForce);
        }

        AudioManager.i.PlaySFX("laser");
    }

    private void EndShoot()
    {
        m_isShooting = false;

        m_animator.SetBool("isShoot", false);
    }

    private void Reload()
    {
        if(m_isShooting) return;
        
        if(m_bulletsLeft < m_magazineSize)
        {
            m_bulletsLeft += (1 / m_bulletsCooldownTime) * Time.deltaTime;
        }
        else
        {
            m_bulletsLeft = m_magazineSize;
        }
    }

    private void OnDragAttackInput(Vector2 direction)
    {
        m_attackInput = direction;
    }

    private void OnPointerUpAttackInput()
    {
        m_shootBufferCounter = m_shootBuffer;
    }

    public void IncreaseMagazineSize()
    {
        if(m_magazineSize >= m_maxMagazineSize) {
            m_bulletsLeft = m_magazineSize;
            return;
        }
        
        m_magazineSize++;
    }
}
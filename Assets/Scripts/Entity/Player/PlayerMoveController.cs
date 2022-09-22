using UnityEngine;

public class PlayerMoveController : MonoBehaviour 
{
    [SerializeField] private Player m_main;
    public Player main => m_main;

    [SerializeField, Range(0f, 30f)] private float m_maxSpeed = 4f;
    public float maxSpeed => m_maxSpeed;
    
    [SerializeField, Range(0f, 1f)] private float m_moveSmooth = .3f;
    [SerializeField, Range(0f, 1f)] private float m_rotationSmooth = .3f;
    
    [SerializeField] private Transform m_root;
    public Transform root => m_root;
    
    private Vector2 m_input;
    private Vector3 m_direction, m_desiredDirection, m_velocity;

    private Animator m_animator;
    private CharacterController m_characterController;
    private PlayerWeaponController m_weaponController;

    private void Start() 
    {
        m_animator = m_main.animator;
        m_characterController = m_main.characterController;
        m_weaponController = m_main.weaponController;
    }

    private void Update()
    {
        if(main.isDie) return;
        
        MoveAction();
    }

    private void MoveAction()
    {   
        m_animator.SetBool("isMove", InputManager.i.MoveInput.Direction.magnitude > 0f);

        m_direction = new Vector3(InputManager.i.MoveInput.Horizontal, 0, InputManager.i.MoveInput.Vertical);
        
        if (!m_characterController.isGrounded)
        {
            m_direction += Physics.gravity;
        }

        m_desiredDirection = Vector3.SmoothDamp(m_desiredDirection, m_direction.normalized, ref m_velocity, m_moveSmooth);
        m_characterController.Move(m_desiredDirection * m_maxSpeed * Time.deltaTime);

        if(m_weaponController.IsShooting)
        {
            LookAt(m_weaponController.shootingDirection);
        }
        else 
        {
            LookAt(m_direction);
        }
    }

    public void LookAt(Vector3 direction)
    {
        Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);

        if(lookDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        m_root.transform.rotation = Quaternion.Slerp(m_root.transform.rotation, targetRotation, m_rotationSmooth);
    }
}
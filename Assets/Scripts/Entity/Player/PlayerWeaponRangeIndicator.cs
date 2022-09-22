using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponRangeIndicator : MonoBehaviour
{
    [SerializeField] private Player m_main;
    public Player main => m_main;

    private PlayerWeaponController m_weaponController;
    
    [SerializeField] private LayerMask m_whatIsEnvironment;

    private Canvas m_canvas;
    private RectTransform m_rectTransform;

    private bool m_isCalculateRange;

    private void Awake() 
    {
        m_canvas = GetComponent<Canvas>();
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void Start() 
    {
        m_weaponController = m_main.weaponController;

        SetActive(false);
        SetRange(m_weaponController.range); 

        InputManager.i.AttackInput.onPointerDown.AddListener(OnPointerDownAttackInput);
        InputManager.i.AttackInput.onDrag.AddListener(OnDragAttackInput); 
        InputManager.i.AttackInput.onPointerUp.AddListener(OnPointerUpAttackInput);
    }

    private void Update() 
    {
        if(!m_isCalculateRange || main.isDie) return;
        
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, m_weaponController.range, m_whatIsEnvironment))
        {
            SetRange(Vector3.Distance(transform.position, hit.point));
        }
        else
        {
            SetRange(m_weaponController.range);
        }
    }

    private void OnPointerDownAttackInput()
    {
        SetActive(true);
        m_isCalculateRange = true;
    }

    private void OnDragAttackInput(Vector2 direction)
    {
        if(direction == Vector2.zero) return;
        
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y), Vector3.up);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void OnPointerUpAttackInput()
    {
        SetActive(false);
        m_isCalculateRange = false;
    }

    public void SetActive(bool val)
    {
        m_canvas.enabled = val;
    }

    public void SetRange(float range)
    {
        m_rectTransform.sizeDelta = new Vector2(m_rectTransform.sizeDelta.x, range);
    }
}

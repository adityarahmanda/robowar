using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class LevelManager : MonoBehaviour 
{
    public static LevelManager i;

    [SerializeField] private Player m_player;
    public Player player => m_player;

    private float m_timeSurvived;
    public float timeSurvived => m_timeSurvived;

    private float m_lastHighScore;
    public float lastHighScore => m_lastHighScore;

    [SerializeField] private CinemachineVirtualCamera m_virtualCamera;

    public bool LevelEnded { get; private set; }
    [HideInInspector] public UnityEvent onLevelEnded;

    private void Awake() 
    {
        if(i == null)
        {
            i = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start() 
    {
        m_virtualCamera.Follow = player.transform;
        m_lastHighScore = PlayerPrefs.GetFloat("highScore");
        LevelEnded = false;
    }

    private void Update() 
    {
        if(LevelEnded) return;

        m_timeSurvived += Time.deltaTime;    
    }

    public void EndLevel()
    {
        LevelEnded = true;

        if(m_timeSurvived > m_lastHighScore)
        {
            PlayerPrefs.SetFloat("highScore", m_timeSurvived);
        }

        onLevelEnded.Invoke();
    }
}
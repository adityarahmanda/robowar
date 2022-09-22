using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject m_mainPanel;
    [SerializeField] private GameObject m_gameOverPanel;
    
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI m_timeSurvivedText;
    [SerializeField] private TextMeshProUGUI m_timeSurvivedScoreText;
    [SerializeField] private TextMeshProUGUI m_highScoreText;

    [SerializeField] private Button m_restartButton;
    [SerializeField] private Button m_mainMenuButton;

    private void Start() 
    {
        m_mainPanel.SetActive(true);
        m_gameOverPanel.SetActive(false);

        LevelManager.i.onLevelEnded.AddListener(() => {
            m_mainPanel.SetActive(false);
            m_gameOverPanel.SetActive(true);

            if(LevelManager.i.timeSurvived > LevelManager.i.lastHighScore)
            {
                m_timeSurvivedScoreText.SetText("New High Score!");
                m_highScoreText.SetText("Time Survived : "  + GetFormattedTimeSurvived() + "s");
            }
            else
            {
                m_timeSurvivedScoreText.SetText("Time Survived : " + GetFormattedTimeSurvived() + "s");
                m_highScoreText.SetText("High Score : "  + GetFormattedHighScore() + "s");
            }
        });

        m_restartButton.onClick.AddListener(() => GameManager.i.StartGame());
        m_mainMenuButton.onClick.AddListener(() => GameManager.i.ReturnToMainMenu());
    }

    private void Update() 
    {
        if(LevelManager.i.LevelEnded) return;

        m_timeSurvivedText.SetText("Time Survived : " + GetFormattedTimeSurvived() + "s");    
    }

    private string GetFormattedTimeSurvived()
    {
        return LevelManager.i.timeSurvived.ToString("0.00");
    }

    private string GetFormattedHighScore()
    {
        return LevelManager.i.lastHighScore.ToString("0.00");
    }
}

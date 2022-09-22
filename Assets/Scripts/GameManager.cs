using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public static GameManager i;

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

    public void StartGame()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
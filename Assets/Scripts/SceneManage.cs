using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Win()
    {
        SceneManager.LoadScene("Victory");
    }

    public void Lose()
    {
        SceneManager.LoadScene("GameOver");
    }
}

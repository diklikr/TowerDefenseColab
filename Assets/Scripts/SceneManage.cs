using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    private void Update()
    {
        // Tecla [ Esc ] o Botón Start del mando (JoystickButton7) abre el menú principal
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            StartMenu();
        }
    }

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

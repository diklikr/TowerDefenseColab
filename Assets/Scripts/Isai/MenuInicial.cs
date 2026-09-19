using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;   // por si venimos de una partida pausada
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
}
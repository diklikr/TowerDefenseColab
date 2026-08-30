using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelGameOver;
    public GameObject panelPausa;

    [Header("Cámara del jugador")]
    public MonoBehaviour scriptCamara;

    private bool juegoTerminado;
    private bool pausado;

    void Start()
    {
        Time.timeScale = 1f;

        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelPausa != null) panelPausa.SetActive(false);

        BloquearCursor(true);
    }

    private void Update()
    {
        if (juegoTerminado) return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (pausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;
        BloquearCursor(false);

        if (panelPausa != null)
            panelPausa.SetActive(true);
    }

    public void Reanudar()
    {
        pausado = false;
        Time.timeScale = 1f;
        BloquearCursor(true);

        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    public void Lose()
    {
        juegoTerminado = true;
        Time.timeScale = 0f;
        BloquearCursor(false);

        if (panelGameOver != null)
            panelGameOver.SetActive(true);
    }

    public void Win()
    {
        juegoTerminado = true;
        Time.timeScale = 0f;
        BloquearCursor(false);
    }

    void BloquearCursor(bool jugando)
    {
        Cursor.lockState = jugando ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !jugando;

        if (scriptCamara != null)
            scriptCamara.enabled = jugando;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}
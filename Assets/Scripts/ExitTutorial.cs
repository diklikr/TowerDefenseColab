using UnityEditor.Rendering;
using UnityEngine;

public class ExitTutorial : MonoBehaviour
{
    public GameObject tutorialUI;
    public void LeaveTutorial()
    {
        tutorialUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

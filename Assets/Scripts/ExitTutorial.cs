using UnityEditor.Rendering;
using UnityEngine;

public class ExitTutorial : MonoBehaviour
{
    public GameObject tutorialUI;
    public void LeaveTutorial()
    {
        tutorialUI.SetActive(false);
    }
}

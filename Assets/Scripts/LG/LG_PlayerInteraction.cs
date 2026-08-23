using System.Resources;
using TMPro;
using UnityEngine;

public class LG_PlayerInteraction : MonoBehaviour
{
    [Header("--- Configuración de Detección ---")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private TextMeshProUGUI promptTextUI; // Texto que muestra "[E] Talar", etc.

    private LG_ResourceNode currentNodeInRange;

    private void Update()
    {
        if (currentNodeInRange != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                // Extraer el recurso y sumarlo al manager
                int amount = currentNodeInRange.Harvest();
                LG_ResourceManager.Instance.AddResource(currentNodeInRange.Type, amount);

                // Si el nodo se destruyó tras la recolección
                if (currentNodeInRange == null)
                {
                    HidePrompt();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LG_ResourceNode>(out var node))
        {
            currentNodeInRange = node;
            ShowPrompt($"[{interactKey}] {node.PromptAction}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<LG_ResourceNode>(out var node) && node == currentNodeInRange)
        {
            currentNodeInRange = null;
            HidePrompt();
        }
    }

    private void ShowPrompt(string message)
    {
        if (promptTextUI != null)
        {
            promptTextUI.text = message;
            promptTextUI.gameObject.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (promptTextUI != null)
        {
            promptTextUI.text = "";
            promptTextUI.gameObject.SetActive(false);
        }
    }
}

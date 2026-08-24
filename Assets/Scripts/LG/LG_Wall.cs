using UnityEngine;

[RequireComponent(typeof(HP))]
public class LG_Wall : MonoBehaviour
{
    [Header("--- Estado del Muro ---")]
    public int level = 1;

    [Header("--- Materiales de Mejora (Opcionales) ---")]
    [SerializeField] private Material level2Material;
    [SerializeField] private Material level3Material;

    private HP hpComponent;
    private Renderer meshRenderer;

    private void Awake()
    {
        // Buscar robustamente en el objeto actual, padres o hijos
        hpComponent = GetComponent<HP>();
        if (hpComponent == null) hpComponent = GetComponentInParent<HP>();
        if (hpComponent == null) hpComponent = GetComponentInChildren<HP>();

        meshRenderer = GetComponent<Renderer>();
        if (meshRenderer == null) meshRenderer = GetComponentInChildren<Renderer>();
        if (meshRenderer == null) meshRenderer = GetComponentInParent<Renderer>();
    }

    public void UpgradeToLevel(int newLevel)
    {
        level = newLevel;

        // Actualizar vida según nivel (Nivel 1 = 1 HP, Nivel 2 = 2 HP, Nivel 3 = 3 HP)
        if (hpComponent != null)
        {
            hpComponent.SetMaxAndCurrentHP(newLevel);
        }

        // Cambiar apariencia visual para retroalimentación
        if (meshRenderer != null)
        {
            Color targetColor = (newLevel == 2) ? new Color(0.45f, 0.24f, 0.08f) : new Color(0.8f, 0.1f, 0.1f);
            Material targetMat = (newLevel == 2) ? level2Material : level3Material;

            if (targetMat != null)
            {
                meshRenderer.material = targetMat;
            }
            else
            {
                // Compatibilidad robusta con URP Lit y Legacy shaders al cambiar color
                Material mat = meshRenderer.material;
                if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor("_BaseColor", targetColor);
                }
                else if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", targetColor);
                }
                else
                {
                    mat.color = targetColor;
                }
            }
        }

        Debug.Log($"Muro mejorado a Nivel {level}. HP asignado: {newLevel}");
    }
}

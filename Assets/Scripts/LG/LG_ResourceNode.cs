using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Mud
}
public class LG_ResourceNode : MonoBehaviour
{

    [Header("--- Configuración de Nodo ---")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amountPerHarvest = 1;
    [SerializeField] private int totalStock = 5; //Cuantas veces se puede extraer antes de destruir
    [SerializeField] string promptAction = "Recolectar";

    public ResourceType Type => resourceType;
    public string PromptAction => promptAction;

    public int Harvest()
    {
        totalStock -= amountPerHarvest;
        int harvested = amountPerHarvest;

        if (totalStock <= 0)
        {
            Destroy(gameObject);
        }
        return harvested;
    }

}

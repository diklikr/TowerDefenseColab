using UnityEngine;
using TMPro;

public class LG_ResourceManager : MonoBehaviour
{
    public static LG_ResourceManager Instance { get; private set; }

    [Header("--- Inventario de Recursos")]
    [SerializeField] private int woodCount = 0;
    [SerializeField] private int stoneCount = 0;
    [SerializeField] private int mudCount = 0;

    [Header("--- Referencias UI (TextMeshPro) ---")]
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI mudText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:
                woodCount += amount;
                break;
            case ResourceType.Stone:
                stoneCount += amount;
                break;
            case ResourceType.Mud:
                mudCount += amount;
                break;
        }
        UpdateUI();
    }

    public bool HasResources(int woodRequired, int stoneRequired, int mudRequired)
    {
        return woodCount >= woodRequired && stoneCount >= stoneRequired && mudCount >= mudRequired;
    }

    public bool ConsumeResources(int woodCost, int stoneCost, int mudCost)
    {
        if (!HasResources(woodCost, stoneCost, mudCost)) return false;

        woodCount -= woodCost;
        stoneCount -= stoneCost;
        mudCount -= mudCost;

        UpdateUI();
        return true;
    }

    private void UpdateUI()
    {
        if (woodText != null) woodText.text = $"Madera: {woodCount}";
        if (stoneText != null) stoneText.text = $"Piedra: {stoneCount}";
        if (mudText != null) mudText.text = $"Lodo: {mudCount}";
    }
}

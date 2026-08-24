using UnityEngine;
using TMPro;

public class LG_ExplorationTracker : MonoBehaviour
{
    [Header("--- Referencias ---")]
    public LG_GridMapGenerator mapGenerator;
    public Transform playerTransform;
    public TextMeshProUGUI explorationTextUI; // Texto para mostrar el % explorado en pantalla

    private int totalWalkableCells = 0;
    private int exploredCells = 0;
    private bool[,] exploredGrid;
    private bool reachedThreshold = false;

    private void Start()
    {
        if (playerTransform == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (mapGenerator == null)
        {
            mapGenerator = FindObjectOfType<LG_GridMapGenerator>();
        }

        Invoke("InitializeTracker", 0.5f); // Esperar a que el mapa se genere
    }

    private void InitializeTracker()
    {
        if (mapGenerator == null) return;

        int w = mapGenerator.width;
        int h = mapGenerator.height;

        exploredGrid = new bool[w, h];
        var cellGrid = mapGenerator.GetCellGrid();

        // Contar celdas transitables (cualquiera que no sea obstáculo)
        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
            {
                if (cellGrid[x, z] != LG_GridMapGenerator.CellType.Obstacle)
                {
                    totalWalkableCells++;
                }
            }
        }

        UpdateExplorationUI();
    }

    private void Update()
    {
        if (mapGenerator == null || playerTransform == null || reachedThreshold) return;

        // Calcular celda en la que está el jugador
        int w = mapGenerator.width;
        int h = mapGenerator.height;
        float size = mapGenerator.cellSize;

        Vector3 offset = new Vector3(w * size / 2f - size / 2f, 0f, h * size / 2f - size / 2f);
        Vector3 localPos = playerTransform.position + offset;

        int playerGridX = Mathf.Clamp(Mathf.RoundToInt(localPos.x / size), 0, w - 1);
        int playerGridZ = Mathf.Clamp(Mathf.RoundToInt(localPos.z / size), 0, h - 1);

        var cellGrid = mapGenerator.GetCellGrid();

        // Si la celda es transitable y no ha sido explorada aún
        if (cellGrid[playerGridX, playerGridZ] != LG_GridMapGenerator.CellType.Obstacle && !exploredGrid[playerGridX, playerGridZ])
        {
            exploredGrid[playerGridX, playerGridZ] = true;
            exploredCells++;

            float percentage = GetExplorationPercentage();
            UpdateExplorationUI();

            Debug.Log($"Zona explorada: {exploredCells}/{totalWalkableCells} ({percentage:F1}%)");

            if (percentage >= 70f)
            {
                reachedThreshold = true;
                OnExplorationThresholdReached();
            }
        }
    }

    public float GetExplorationPercentage()
    {
        if (totalWalkableCells == 0) return 0f;
        return ((float)exploredCells / totalWalkableCells) * 100f;
    }

    private void UpdateExplorationUI()
    {
        if (explorationTextUI != null)
        {
            explorationTextUI.text = $"Explorado: {GetExplorationPercentage():F1}% / 70%";
        }
    }

    private void OnExplorationThresholdReached()
    {
        Debug.LogWarning("¡ALCANZADO EL 70% DE EXPLORACIÓN REQUERIDO POR EL GDD!");
        if (explorationTextUI != null)
        {
            explorationTextUI.text = "¡70% Exploración Alcanzada! Zonas oscuras despejadas.";
            explorationTextUI.color = Color.green;
        }
    }
}

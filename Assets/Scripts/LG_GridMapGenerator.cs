using System.Collections.Generic;
using UnityEngine;

public class LG_GridMapGenerator : MonoBehaviour
{
    [Header("--- Dimensiones del Grid ---")]
    public int width = 15;
    public int height = 15;
    public float cellSize = 10f;

    [Header("--- Prefabs ---")]
    public GameObject basePrefab;
    public GameObject obstaclePrefab;
    public GameObject woodNodePrefab;
    public GameObject stoneNodePrefab;
    public GameObject mudNodePrefab;

    [Header("--- Densidades (%) ---")]
    [Range(0f, 0.3f)] public float obstacleDensity = 0.15f;
    [Range(0f, 0.1f)] public float resourceDensity = 0.05f;

    private CellType[,] grid;
    private bool[,] visited;
    
    public enum CellType { Empty, Obstacle, Resource, Base }

    public struct GridPos
    {
        public int x;
        public int z;
        public GridPos(int x, int z) { this.x = x; this.z = z; }
    }

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        grid = new CellType[width, height];
        
        // 1. Inicializar todo vacío
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                grid[x, z] = CellType.Empty;
            }
        }

        // 2. Colocar la Base en el centro
        int centerX = width / 2;
        int centerZ = height / 2;
        grid[centerX, centerZ] = CellType.Base;

        // 3. Colocar Obstáculos y Recursos de forma aleatoria
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // No sobrescribir la base
                if (x == centerX && z == centerZ) continue;

                float rand = Random.value;
                if (rand < obstacleDensity)
                {
                    grid[x, z] = CellType.Obstacle;
                }
                else if (rand < obstacleDensity + resourceDensity)
                {
                    grid[x, z] = CellType.Resource;
                }
            }
        }

        // 4. Asegurar conectividad (Flood Fill/BFS desde el centro)
        FloodFillConnectivity(centerX, centerZ);

        // 5. Instanciar en la escena
        InstantiateGridElements(centerX, centerZ);
    }

    private void FloodFillConnectivity(int startX, int startZ)
    {
        visited = new bool[width, height];
        Queue<GridPos> queue = new Queue<GridPos>();
        queue.Enqueue(new GridPos(startX, startZ));
        visited[startX, startZ] = true;

        int[] dx = { 0, 0, 1, -1 };
        int[] dz = { 1, -1, 0, 0 };

        while (queue.Count > 0)
        {
            GridPos curr = queue.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int nx = curr.x + dx[i];
                int nz = curr.z + dz[i];

                if (nx >= 0 && nx < width && nz >= 0 && nz < height)
                {
                    // Se puede pasar si no es un obstáculo y no ha sido visitado
                    if (!visited[nx, nz] && grid[nx, nz] != CellType.Obstacle)
                    {
                        visited[nx, nz] = true;
                        queue.Enqueue(new GridPos(nx, nz));
                    }
                }
            }
        }

        // Cualquier celda vacía o con recurso que no se haya visitado, 
        // se convierte en obstáculo para evitar espacios muertos / inaccesibles.
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (!visited[x, z] && grid[x, z] != CellType.Obstacle)
                {
                    grid[x, z] = CellType.Obstacle;
                }
            }
        }
    }

    private void InstantiateGridElements(int centerX, int centerZ)
    {
        Vector3 offset = new Vector3(width * cellSize / 2f - cellSize / 2f, 0f, height * cellSize / 2f - cellSize / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0f, z * cellSize) - offset;
                
                // Ajustar la altura según el terreno (raycast al suelo para colocar sobre la superficie)
                RaycastHit hit;
                if (Physics.Raycast(pos + Vector3.up * 10f, Vector3.down, out hit, 20f))
                {
                    pos.y = hit.point.y ;
                }

                CellType cell = grid[x, z];

                if (cell == CellType.Base && basePrefab != null)
                {
                    GameObject b = Instantiate(basePrefab, pos, Quaternion.identity);
                    b.tag = "Base";
                }
                else if (cell == CellType.Obstacle && obstaclePrefab != null)
                {
                    Instantiate(obstaclePrefab, pos, Quaternion.identity);
                }
                else if (cell == CellType.Resource)
                {
                    GameObject resourceToSpawn = ChooseRandomResourcePrefab();
                    if (resourceToSpawn != null)
                    {
                        Instantiate(resourceToSpawn, pos, Quaternion.identity);
                    }
                }
            }
        }
    }

    private GameObject ChooseRandomResourcePrefab()
    {
        float rand = Random.value;
        if (rand < 0.3f && woodNodePrefab != null) return woodNodePrefab;
        if (rand < 0.6f && stoneNodePrefab != null) return stoneNodePrefab;
        return mudNodePrefab;
    }

    public bool[,] GetVisitedGrid() => visited;
    public CellType[,] GetCellGrid() => grid;
}

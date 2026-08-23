using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [Header("--- Configuración de Construcción ---")]
    public GameObject wallPrefab;
    public Transform buildPoint;
    public float buildDistance = 5f;
    public Transform playerCamera;

    private void Start()
    {
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        // Tecla [ B ] para construir nuevo muro
        if (Input.GetKeyDown(KeyCode.B))
        {
            TryBuildNewWall();
        }

        // Tecla [ U ] para mejorar el muro al que estás apuntando
        if (Input.GetKeyDown(KeyCode.U))
        {
            TryUpgradeLookedWall();
        }
    }

    void TryUpgradeLookedWall()
    {
        RaycastHit hit;
        Transform cam = playerCamera != null ? playerCamera : transform;
        
        bool hitSomething = false;
        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, buildDistance);
        
        // Buscar el primer impacto que no sea el propio jugador
        RaycastHit actualHit = new RaycastHit();
        foreach (var candidateHit in hits)
        {
            if (candidateHit.collider.CompareTag("Player")) continue;
            actualHit = candidateHit;
            hitSomething = true;
            break;
        }

        if (hitSomething)
        {
            GameObject hitObj = actualHit.collider.gameObject;
            
            // Buscar hacia arriba de forma recursiva en la jerarquía para ver si algún ancestro tiene el tag "Escudo"
            Transform current = hitObj.transform;
            GameObject wallRoot = null;
            while (current != null)
            {
                if (current.CompareTag("Escudo"))
                {
                    wallRoot = current.gameObject;
                    break;
                }
                current = current.parent;
            }

            // Si encontramos un objeto con el tag "Escudo" en la jerarquía
            if (wallRoot != null)
            {
                // Buscar componente Wall en el objeto raíz del muro
                Wall wallComponent = wallRoot.GetComponent<Wall>();
                if (wallComponent == null)
                {
                    wallComponent = wallRoot.AddComponent<Wall>();
                    wallComponent.level = 1;
                }

                TryUpgradeWall(wallComponent);
            }
            else
            {
                // Imprimir el nombre y el tag del objeto impactado para facilitar la depuración
                Debug.LogWarning($"No estás apuntando a un muro para mejorar. Impactaste a: '{hitObj.name}' (Tag: '{hitObj.tag}').");
            }
        }
        else
        {
            Debug.LogWarning("No se detectó ningún objeto al intentar mejorar (fuera de rango o sin colisión).");
        }
    }

    void TryUpgradeWall(Wall wall)
    {
        if (wall.level == 1)
        {
            // Nivel 2: Costo 2 Lodo, 1 Piedra
            if (LG_ResourceManager.Instance != null)
            {
                if (LG_ResourceManager.Instance.ConsumeResources(0, 1, 2)) // Madera, Piedra, Lodo
                {
                    wall.UpgradeToLevel(2);
                }
                else
                {
                    Debug.LogWarning("Recursos insuficientes para Muro Nivel 2 (2 Lodo, 1 Piedra).");
                }
            }
        }
        else if (wall.level == 2)
        {
            // Nivel 3: Costo 1 Piedra, 1 Lodo
            if (LG_ResourceManager.Instance != null)
            {
                if (LG_ResourceManager.Instance.ConsumeResources(0, 1, 1)) // Madera, Piedra, Lodo
                {
                    wall.UpgradeToLevel(3);
                }
                else
                {
                    Debug.LogWarning("Recursos insuficientes para Muro Nivel 3 (1 Piedra, 1 Lodo).");
                }
            }
        }
        else
        {
            Debug.Log("El muro ya está al nivel máximo (Nivel 3).");
        }
    }

    void TryBuildNewWall()
    {
        // Nivel 1: Costo 2 Madera, 3 Piedra
        if (LG_ResourceManager.Instance != null)
        {
            if (LG_ResourceManager.Instance.ConsumeResources(2, 3, 0)) // Madera, Piedra, Lodo
            {
                if (wallPrefab != null && buildPoint != null)
                {
                    GameObject newWall = Instantiate(wallPrefab, buildPoint.position, buildPoint.rotation);
                    newWall.tag = "Escudo"; // Asegurar que tenga el tag correspondiente

                    Wall wallComp = newWall.GetComponent<Wall>();
                    if (wallComp == null)
                    {
                        wallComp = newWall.AddComponent<Wall>();
                    }
                    wallComp.level = 1;

                    Debug.Log("Nuevo muro Nivel 1 construido.");
                }
                else
                {
                    Debug.LogError("wallPrefab o buildPoint no configurados en el inspector de BuildSystem.");
                }
            }
            else
            {
                Debug.LogWarning("Recursos insuficientes para construir Muro Nivel 1 (2 Madera, 3 Piedra).");
            }
        }
    }
}
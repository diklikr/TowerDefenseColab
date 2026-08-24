using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [Header("--- ConfiguraciÃ³n de ConstrucciÃ³n ---")]
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
        // Tecla [ B ] o BotÃ³n A / Cruz del mando (JoystickButton0) para construir nuevo muro
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            TryBuildNewWall();
        }

        // Tecla [ U ] o BotÃ³n Y / TriÃ¡ngulo del mando (JoystickButton3) para mejorar el muro al que estÃ¡s apuntando
        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.JoystickButton3))
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
            
            // Buscar hacia arriba de forma recursiva en la jerarquÃ­a para ver si algÃºn ancestro tiene el tag "Escudo"
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

            // Si encontramos un objeto con el tag "Escudo" en la jerarquÃ­a
            if (wallRoot != null)
            {
                // Buscar componente LG_Wall en el objeto raÃ­z del muro
                LG_Wall wallComponent = wallRoot.GetComponent<LG_Wall>();
                if (wallComponent == null)
                {
                    wallComponent = wallRoot.AddComponent<LG_Wall>();
                    wallComponent.level = 1;
                }

                TryUpgradeWall(wallComponent);
            }
            else
            {
                // Imprimir el nombre y el tag del objeto impactado para facilitar la depuraciÃ³n
                Debug.LogWarning($"No estÃ¡s apuntando a un muro para mejorar. Impactaste a: '{hitObj.name}' (Tag: '{hitObj.tag}').");
            }
        }
        else
        {
            Debug.LogWarning("No se detectÃ³ ningÃºn objeto al intentar mejorar (fuera de rango o sin colisiÃ³n).");
        }
    }

    void TryUpgradeWall(LG_Wall LG_Wall)
    {
        if (LG_Wall.level == 1)
        {
            // Nivel 2: Costo 2 Lodo, 1 Piedra
            if (LG_ResourceManager.Instance != null)
            {
                if (LG_ResourceManager.Instance.ConsumeResources(0, 1, 2)) // Madera, Piedra, Lodo
                {
                    LG_Wall.UpgradeToLevel(2);
                }
                else
                {
                    Debug.LogWarning("Recursos insuficientes para Muro Nivel 2 (2 Lodo, 1 Piedra).");
                }
            }
        }
        else if (LG_Wall.level == 2)
        {
            // Nivel 3: Costo 1 Piedra, 1 Lodo
            if (LG_ResourceManager.Instance != null)
            {
                if (LG_ResourceManager.Instance.ConsumeResources(0, 1, 1)) // Madera, Piedra, Lodo
                {
                    LG_Wall.UpgradeToLevel(3);
                }
                else
                {
                    Debug.LogWarning("Recursos insuficientes para Muro Nivel 3 (1 Piedra, 1 Lodo).");
                }
            }
        }
        else
        {
            Debug.Log("El muro ya estÃ¡ al nivel mÃ¡ximo (Nivel 3).");
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

                    LG_Wall wallComp = newWall.GetComponent<LG_Wall>();
                    if (wallComp == null)
                    {
                        wallComp = newWall.AddComponent<LG_Wall>();
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

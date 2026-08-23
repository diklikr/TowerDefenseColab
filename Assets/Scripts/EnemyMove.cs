using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject house;
    public GameObject escudo;
    public float speed = 5f;

    private GameObject target;

    void Start()
    {
        if (house == null)
        {
            house = GameObject.FindGameObjectWithTag("Base");
        }
        ElegirObjetivo();
    }

    void Update()
    {
        if (target == null)
        {
            ElegirObjetivo();
            if (target == null) return;
        }

        // Moverse hacia el objetivo
        Vector3 targetPos = target.transform.position;
        targetPos.y = transform.position.y; // Mantener la misma altura para evitar inclinaciones
        
        Vector3 dir = (targetPos - transform.position).normalized;
        Vector3 moveDir = dir;

        // Evitado de obstáculos simple usando raycast
        RaycastHit hit;
        float rayLength = 2.5f;

        // Proyectar un rayo hacia adelante
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength))
        {
            // Si el obstáculo no es nuestro objetivo directo y es un escudo/muro u otro obstáculo
            if (hit.collider.gameObject != target)
            {
                // Esquivar apartándose de la normal del impacto
                Vector3 hitNormal = hit.normal;
                hitNormal.y = 0f; // Evitar movimientos verticales

                // Dirección de evasión
                moveDir = (dir + hitNormal * 2.5f).normalized;

                // Rotar suavemente hacia la dirección de evasión
                if (moveDir != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 5f);
                }
            }
        }
        else
        {
            // Rotar suavemente hacia el objetivo
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
            }
        }

        // Desplazar al enemigo en la dirección final calculada
        transform.position += moveDir * speed * Time.deltaTime;

        // Verificar si llegó
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            target = null;
        }
    }

    void ElegirObjetivo()
    {
        if (house == null) return;

        float distCasa = Vector3.Distance(transform.position, house.transform.position);
        GameObject escudoCercano = BuscarEscudo();

        if (escudoCercano != null && Vector3.Distance(transform.position, escudoCercano.transform.position) < distCasa)
        {
            target = escudoCercano;
        }
        else
        {
            target = house;
        }
    }

    GameObject BuscarEscudo()
    {
        GameObject[] escudos = GameObject.FindGameObjectsWithTag("Escudo");
        GameObject mas_cercano = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in escudos)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                mas_cercano = e;
            }
        }
        return mas_cercano;
    }
}

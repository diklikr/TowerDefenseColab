using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject house;
    public GameObject escudo;
    public float speed = 5f;

    private GameObject target;

    void Start()
    {
        target = escudo;
    }

    void Update()
    {
        if (target == null)
        {
            ElegirObjetivo();
            return;
        }

        // Moverse hacia el objetivo
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // Verificar si llegó
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target = null;
        }
    }

    void ElegirObjetivo()
    {
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

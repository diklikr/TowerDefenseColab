using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemigoIA : MonoBehaviour
{
    public Transform casa;

    [Header("Rangos persecución")]
    public float rangoDeteccion = 8f;   // desde aquí persigue al jugador
    public float rangoPerdida = 14f;   // aquí lo suelta y vuelve al muro

    NavMeshAgent agente;
    Transform jugador;
    Transform objetivo;
    float proximaBusqueda;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.stoppingDistance = 0f;   // que se meta al objetivo hasta chocar

        if (casa == null)
        {
            GameObject bas = GameObject.FindGameObjectWithTag("Base");
            if (bas != null) casa = bas.transform;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) jugador = player.transform;

        objetivo = BuscarEstructura();
    }

    void Update()
    {
        ElegirObjetivo();

        if (objetivo != null)
            agente.SetDestination(objetivo.position);
    }

    void ElegirObjetivo()
    {
        float distJugador = (jugador == null) ? Mathf.Infinity : Distancia(jugador);

        // Ya lo estoy persiguiendo: solo reviso si se escapó
        if (objetivo == jugador && jugador != null)
        {
            if (distJugador > rangoPerdida)
                objetivo = BuscarEstructura();
            return;
        }

        // Se acercó demasiado: lo persigo
        if (distJugador < rangoDeteccion)
        {
            objetivo = jugador;
            return;
        }

        // Voy por estructuras: refresco el objetivo cada medio segundo
        if (objetivo == null || Time.time >= proximaBusqueda)
        {
            proximaBusqueda = Time.time + 0.5f;
            objetivo = BuscarEstructura();
        }
    }

    Transform BuscarEstructura()
    {
        GameObject[] escudos = GameObject.FindGameObjectsWithTag("Escudo");
        Transform cercano = casa;
        float minDist = (casa == null) ? Mathf.Infinity : Distancia(casa);

        foreach (GameObject e in escudos)
        {
            float d = Distancia(e.transform);
            if (d < minDist)
            {
                minDist = d;
                cercano = e.transform;
            }
        }
        return cercano;
    }

    float Distancia(Transform t)
    {
        Collider col = t.GetComponent<Collider>();
        Vector3 punto = (col != null) ? col.ClosestPoint(transform.position) : t.position;
        return Vector3.Distance(transform.position, punto);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoPerdida);
    }
}
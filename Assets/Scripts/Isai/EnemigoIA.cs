using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemigoIA : MonoBehaviour
{
    [Header("Objetivos")]
    public Transform casa;

    [Header("Rangos")]
    public float rangoDeteccion;   // desde aquí persigue al jugador
    public float rangoPerdida;   // aquí lo suelta y vuelve al muro
    public float distanciaImpacto = 0.8f;

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
            GameObject b = GameObject.FindGameObjectWithTag("Base");
            if (b != null) casa = b.transform;
        }

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jugador = p.transform;

        objetivo = BuscarEstructura();
    }

    void Update()
    {
        ElegirObjetivo();
        if (objetivo == null) return;

        if (objetivo != jugador && Distancia(objetivo) <= distanciaImpacto)
        {
            GetComponent<EnemyHP>().Impactar(objetivo.gameObject);
            return;
        }

        agente.SetDestination(PuntoDestino(objetivo));
    }

    void ElegirObjetivo()
    {
        float distJugador = (jugador == null) ? Mathf.Infinity : Distancia(jugador);

        //Cuando lo persigue al jugador detecta si salió del rango
        if (objetivo == jugador && jugador != null)
        {
            if (distJugador > rangoPerdida)
                objetivo = BuscarEstructura();
            return;
        }

        //Detecta si el jugador esta dentro del rango
        if (distJugador < rangoDeteccion)
        {
            objetivo = jugador;
            return;
        }

        //Busca un muro disponible para atacar
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
    Vector3 PuntoDestino(Transform t)
    {
        Collider col = t.GetComponent<Collider>();
        return (col != null) ? col.ClosestPoint(transform.position) : t.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoPerdida);
    }
}
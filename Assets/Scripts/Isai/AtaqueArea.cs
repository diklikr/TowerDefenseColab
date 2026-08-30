using UnityEngine;

public class AtaqueArea : MonoBehaviour
{
    public float radio = 5f;
    public int dano = 1;
    public float cooldown = 3f;
    public KeyCode tecla = KeyCode.Space;

    private float proximoAtaque;

    //Esto solo lo lee la UI
    public float PorcentajeCooldown
    {
        get
        {
            float restante = proximoAtaque - Time.time;
            if (restante <= 0f) return 0f;
            return restante / cooldown;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(tecla) && Time.time >= proximoAtaque)
        {
            Atacar();
            proximoAtaque = Time.time + cooldown;
        }
    }

    void Atacar()
    {
        //Busca todos los colliders dentro del radio
        Collider[] cercanos = Physics.OverlapSphere(transform.position, radio);

        foreach (Collider c in cercanos)
        {
            EnemyHP enemigo = c.GetComponent<EnemyHP>();
            if (enemigo == null) enemigo = c.GetComponentInParent<EnemyHP>();

            if (enemigo != null)
                enemigo.TakeDamage(dano);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
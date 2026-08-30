using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public static int enemigosVivos = 0;   // contador compartido por todos los enemigos

    public int startHP = 1;
    public int damage = 1;
    [SerializeField] private int currentHP;

    private void Start()
    {
        currentHP = startHP;
        enemigosVivos++;
    }

    private void OnDestroy()
    {
        enemigosVivos--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // This validates if the enemy hits a shield or the base
        if (collision.gameObject.CompareTag("Escudo") || collision.gameObject.CompareTag("Base"))
        {
            HP targetHealth = collision.gameObject.GetComponent<HP>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
            TakeDamage(damage);
        }
        // This validates if the enemy hits the player to drain sanity
        else if (collision.gameObject.CompareTag("Player"))
        {
            LG_PlayerSanity playerSanity = collision.gameObject.GetComponent<LG_PlayerSanity>();
            if (playerSanity != null)
            {
                playerSanity.TakeSanityDamage(2f);
            }
            TakeDamage(damage);
        }
    }

    //Daño directo por cercanía, lo llama EnemigoIA
    public void Impactar(GameObject objetivo)
    {
        HP vida = objetivo.GetComponent<HP>();
        if (vida == null) vida = objetivo.GetComponentInParent<HP>();

        if (vida != null)
            vida.TakeDamage(damage);

        TakeDamage(damage);
    }

    public void TakeDamage(int incomingDamage)
    {
        // Updates health correctly
        currentHP -= incomingDamage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
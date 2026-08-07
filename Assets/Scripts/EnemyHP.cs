using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int startHP = 1;
    public int damage = 1;
    private int currentHP;

    private void Start()
    {
        currentHP = startHP;
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
            PlayerSanity playerSanity = collision.gameObject.GetComponent<PlayerSanity>();
            if (playerSanity != null)
            {
                playerSanity.DrainSanityTemporary(2);
            }
            TakeDamage(damage);
        }
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
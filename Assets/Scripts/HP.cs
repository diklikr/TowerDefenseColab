using UnityEngine;

public class HP : MonoBehaviour
{
    public SceneManage sceneManage;
    public int startHP = 1;
    private int currentHealth;

    private void Start()
    {
        ResetHP();
    }

    public void ResetHP()
    {
        currentHealth = startHP;
    }

    public void TakeDamage(int incomingDamage)
    {
        // Subtracts incoming damage from current health
        currentHealth -= incomingDamage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroys the shield or triggers game over if the base falls
        if (gameObject.CompareTag("Escudo"))
        {
            Destroy(gameObject);
        }
        else if (gameObject.CompareTag("Base"))
        {
            if (sceneManage != null)
            {
                sceneManage.Lose();
            }
        }
    }
}
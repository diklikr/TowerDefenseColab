using UnityEngine;

public class HP : MonoBehaviour
{
    public SceneManage sceneManage;
    public int startHP = 1;
    
    [SerializeField] 
    private int currentHealth;

    private void Start()
    {
        ResetHP();
        if (sceneManage == null)
        {
            sceneManage = FindAnyObjectByType<SceneManage>();
        }
    }

    public void ResetHP()
    {
        currentHealth = startHP;
    }

    public void SetMaxAndCurrentHP(int hp)
    {
        startHP = hp;
        currentHealth = hp;
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
        //Si la base se destruye, lanza el gameover antes de destruirse
        if (gameObject.CompareTag("Base") && sceneManage != null)
        {
            sceneManage.Lose();
        }

        Destroy(gameObject);
    }
}
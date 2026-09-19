using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public SceneManage sceneManage;
    public int startHP = 10;
    public Slider slider;
    
    [SerializeField] 
    private int currentHealth;

    private void Start()
    {
        slider = GameObject.FindGameObjectWithTag("HPslider").GetComponent<Slider>();
        ResetHP();
        if(slider != null )
        {
            Debug.Log("no slider");
        }
     
        if (sceneManage == null)
        {
            sceneManage = FindAnyObjectByType<SceneManage>();
        }
    }

    public void ResetHP()
    {
        currentHealth = startHP;
        UpdateSlider();
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
        UpdateSlider();
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

    private void UpdateSlider()
    {
        slider.value = currentHealth;
    }
}
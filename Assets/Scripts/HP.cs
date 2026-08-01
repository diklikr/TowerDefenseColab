using JetBrains.Annotations;
using UnityEngine;

public class HP : MonoBehaviour
{
    public int startHP;
    int crrhealth;
    int damage;

    private void Start()
    {
        ResetHP();
    }
    public void ResetHP()
    {
        crrhealth = startHP;
    }
    public void TakeDamage(int damage)
    {
        crrhealth = damage - crrhealth;
        if (crrhealth > 0)
        {
            Die();
        }
    }
    void Die()
    {
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(CompareTag("Enemy"))
        {
            TakeDamage(damage);
        }
    }
}
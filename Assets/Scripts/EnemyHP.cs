using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    int HP;
    public int startHP;

    private void Start()
    {
        HP = startHP;
    }

    public void TakeDamage(int damage)
    {
        HP = damage - HP;
        if (HP > 0) { Die(); }
        return;
    }

    void Die()
        {
            Destroy(gameObject);
        }
}

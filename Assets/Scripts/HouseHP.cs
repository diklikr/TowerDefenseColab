using UnityEngine;

public class HouseHP : MonoBehaviour
{
    public int startHP;
    int crrhealth;

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
    }
}
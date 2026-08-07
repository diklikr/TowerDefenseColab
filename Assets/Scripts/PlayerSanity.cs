using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    public int maxSanity = 10;
    private int currentSanity;

    void Start()
    {
        currentSanity = maxSanity;
    }

    public void DrainSanityTemporary(int amount)
    {
        // This simulates temporary sanity loss on enemy impact
        currentSanity -= amount;
        if (currentSanity < 0)
        {
            currentSanity = 0;
        }

        Debug.Log("Sanity dropped! Current Sanity: " + currentSanity);
        // Note: Logic for recovering this temporary sanity over time will go here
    }
}
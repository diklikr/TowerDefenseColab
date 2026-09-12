using UnityEngine;

public class FOG : MonoBehaviour
{
    public LG_PlayerSanity sanity;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            sanity.RemoveSanity();
            Destroy(gameObject);
        }
    }
}

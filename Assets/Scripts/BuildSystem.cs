using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public GameObject wallPrefab;
    public Transform buildPoint;

    void Update()
    {
        // This triggers wall placement when the player presses E
        if (Input.GetKeyDown(KeyCode.E))
        {
            BuildWall();
        }
    }

    void BuildWall()
    {
        if (wallPrefab != null && buildPoint != null)
        {
            Instantiate(wallPrefab, buildPoint.position, buildPoint.rotation);
        }
    }
}
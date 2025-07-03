using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera camera;
    public Transform player;
    public float ceilingCap = 3f;
    public float floorCap = -3f; 

    void Awake()
    {
        camera = GetComponent<Camera>();
    }
    void LateUpdate() 
    {
        var pos = new Vector3(player.position.x, transform.position.y, transform.position.z);
        // calc ceiling
        float ceiling = transform.position.y + camera.orthographicSize - ceilingCap;
        float ceilDelta = player.position.y - ceiling;
        if (ceilDelta > 0) 
        {
            pos.y += ceilDelta;
        }

        float floor   = transform.position.y - camera.orthographicSize - floorCap;
        float floorDelta = player.position.y - floor;
        if (floorDelta < 0) 
        {
            pos.y += floorDelta;
        }
        transform.position = pos;
    }
}

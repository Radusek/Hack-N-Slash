using UnityEngine;

public class PlayerToWorldBinder : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<HUDManager>().SetPlayer(GetComponent<EntityStats>());
        FindObjectOfType<CameraController>().SetObjectToFollow(transform);
        FindObjectOfType<HorizontalCameraFollow>().SetObjectToFollow(transform);
        FindObjectOfType<PlayerSpawn>().SetPlayer(transform);
        Destroy(this);
    }
}

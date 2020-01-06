using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDBinder : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<HUDManager>().SetPlayer(GetComponent<EntityStats>());
        FindObjectOfType<CameraController>().SetObjectToFollow(transform);
        Destroy(this);
    }
}

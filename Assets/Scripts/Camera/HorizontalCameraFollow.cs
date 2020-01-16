using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform objectToFollow;

    [SerializeField]
    private Vector3 positionOffset;

    public void SetObjectToFollow(Transform newObjectToFollow)
    {
        objectToFollow = newObjectToFollow;
    }

    void Update()
    {
        transform.position = objectToFollow.position + positionOffset;
    }
}

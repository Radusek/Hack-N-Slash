using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera camera;
    [SerializeField]
    private LayerMask raycastMask;

    [SerializeField]
    private Transform objectToFollow;

    [SerializeField]
    private Vector3 positionOffset;

    [SerializeField]
    [Range(0f, 1f)]
    private float cursorFactor = 0.5f;

    [SerializeField]
    private float maxCameraMovingDistance = 3f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, raycastMask))
            return;

        hit.point = new Vector3(hit.point.x, objectToFollow.position.y, hit.point.z);
        Vector3 deltaPos = cursorFactor * (hit.point - objectToFollow.position);
        deltaPos.y = 0f;

        if (deltaPos.magnitude >= maxCameraMovingDistance)
            deltaPos = maxCameraMovingDistance * deltaPos.normalized;

        transform.position = objectToFollow.position;

        transform.position += deltaPos + positionOffset;
    }

    public void SetObjectToFollow(Transform target)
    {
        objectToFollow = target;
    }
}

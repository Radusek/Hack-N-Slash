using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private GameObject playerGraphicsObject;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask raycastMask;

    [SerializeField]
    private float movementSpeed = 1f;

    private Vector2 movementInput;

    private Vector3 lookingPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ReadInput();
        Rotate();
    }

    private void ReadInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput.Normalize();

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastMask))
            lookingPosition = hit.point;
    }

    private void Rotate()
    {
        lookingPosition.y = playerGraphicsObject.transform.position.y;
        playerGraphicsObject.transform.LookAt(lookingPosition);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 newVelocity;
        newVelocity.x = movementSpeed * movementInput.x;
        newVelocity.y = rb.velocity.y;
        newVelocity.z = movementSpeed * movementInput.y;

        rb.velocity = newVelocity;
    }
}
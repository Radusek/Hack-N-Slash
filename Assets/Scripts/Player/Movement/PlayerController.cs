using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask raycastMask;

    [SerializeField]
    private float movementSpeed = 1f;

    private Vector2 movementInput;

    private Vector3 lookingPosition;

    public DirectionEvent OnMovementDirectionChanged;
    private Direction lastDirection = Direction.None;

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
        lookingPosition.y = transform.position.y;
        transform.LookAt(lookingPosition);
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

        rb.velocity = Vector3.Lerp(rb.velocity, newVelocity, movementSpeed * 2 * Time.deltaTime);

        Direction currentDirection = GetDirection();
        if (currentDirection != lastDirection)
        {
            OnMovementDirectionChanged?.Invoke(currentDirection);
            lastDirection = currentDirection;
        }
    }

    private Direction GetDirection()
    {
        Vector3 velocity = rb.velocity;

        if (velocity.sqrMagnitude < 0.01f)
            return Direction.None;

        float deltaDegrees = transform.forward.AngleDegreesBetween(velocity);

        if (deltaDegrees <= 45f)
            return Direction.Forward;
        else if (deltaDegrees < 135f)
        {
            if (Vector3.Cross(transform.forward, velocity).y < 0f)
                return Direction.Left;
            else
                return Direction.Right;
        }
        else return Direction.Back;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public void AddMovementSpeed(float speed)
    {
        movementSpeed += speed;
    }
}
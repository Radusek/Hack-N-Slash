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

    [SerializeField]
    [Range(0f, 1f)]
    private float lookingDirectionFactor = 0.5f;

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
        Vector3 lookingDirection = transform.rotation * Vector3.forward;

        Vector3 inputVelocity;
        inputVelocity.x = movementSpeed * movementInput.x;
        inputVelocity.y = 0f;
        inputVelocity.z = movementSpeed * movementInput.y;

        float angleRadians = inputVelocity.AngleDegreesBetween(lookingDirection) * Mathf.Deg2Rad;

        Vector3 newVelocity = inputVelocity * (Mathf.Cos(angleRadians*0.5f)* lookingDirectionFactor + (1f - lookingDirectionFactor));
        newVelocity.y = rb.velocity.y;

        Debug.Log(newVelocity.magnitude);

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

    public Vector3 GetLookingPosition()
    {
        return lookingPosition;
    }
}
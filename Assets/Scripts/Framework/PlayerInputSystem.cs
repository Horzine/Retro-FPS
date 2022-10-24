using System;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    public float MouseAxisX { get; private set; }
    public float MouseAxisY { get; private set; }
    public Vector2 MovementDirection { get; private set; }
    public float MouseSensitivity { get; private set; } = 250;
    public event Action FireAction;
    public event Action SecondaryFireAction;

    private void Update()
    {
        HandleMouseInput();

        HandleFireInput();

        HandleMoveDirectionInput();
    }

    private void HandleMouseInput()
    {
        MouseAxisX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        MouseAxisY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
    }

    private void HandleFireInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireAction?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SecondaryFireAction?.Invoke();
        }
    }

    private void HandleMoveDirectionInput()
    {
        MovementDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            MovementDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MovementDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            MovementDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MovementDirection += Vector2.right;
        }
        MovementDirection.Normalize();
    }
}

using System;
using UnityEngine;

namespace Framework
{
    public class PlayerInputSystem : MonoSingleton<PlayerInputSystem>
    {
        public float MouseAxisX { get; private set; }
        public float MouseAxisY { get; private set; }
        public Vector2 MovementDirection { get; private set; }
        public float MouseSensitivity { get; private set; } = 250;
        public event Action FireActionDown;
        public event Action FireActionUp;
        public event Action FireActionPress;
        public event Action SecondaryFireAction;
        public event Action ReloadAction;
        public event Action<bool> SwapWeaponAction; // bool == true is "IsSwapNext"

        private void Update()
        {
            HandleMouseInput();

            HandleFireInput();

            HandleSwapWeaponInput();

            HandleReloadInput();

            HandleMoveDirectionInput();

            HandleCursorLockModeChange();
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
                FireActionDown?.Invoke();
            }
            if (Input.GetMouseButton(0))
            {
                FireActionPress?.Invoke();
            }
            if (Input.GetMouseButtonUp(0))
            {
                FireActionUp?.Invoke();
            }

            if (Input.GetMouseButtonDown(1))
            {
                SecondaryFireAction?.Invoke();
            }
        }

        private void HandleReloadInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadAction?.Invoke();
            }
        }

        private void HandleSwapWeaponInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SwapWeaponAction?.Invoke(true);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                SwapWeaponAction?.Invoke(false);
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

        private void HandleCursorLockModeChange()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                var cursorLockMode = Cursor.lockState;
                Cursor.lockState = cursorLockMode == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }

        public void SetCurrentMouseCursorLockMode(CursorLockMode mode)
        {
            Cursor.lockState = mode;
        }
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputSystem InputSystem;
    public Transform PlayerCameraTsf;
    private Transform _selfTsf;
    public float Speed;
    private float _cameraRotation;

    private void Awake()
    {
        _selfTsf = transform;
    }

    private void Start()
    {
        InputSystem.FireAction += OnFire;
        InputSystem.SecondaryFireAction += SecondaryFire;
    }

    private void OnFire()
    {
        // throw new NotImplementedException();
    }

    private void SecondaryFire()
    {
        // throw new NotImplementedException();
    }

    private void Update()
    {
        HandleMovementInput();

        HandleMouseAxisInput();
    }

    private void HandleMovementInput()
    {
        var dir = Speed * Time.deltaTime * InputSystem.MovementDirection;
        _selfTsf.Translate(new Vector3(dir.x, 0, dir.y), Space.Self);
    }
    public void HandleMouseAxisInput()
    {
        _cameraRotation = Mathf.Clamp(_cameraRotation - InputSystem.MouseAxisY, -90, 90);
        PlayerCameraTsf.localRotation = Quaternion.Euler(_cameraRotation, 0, 0);

        _selfTsf.Rotate(new Vector3(0, InputSystem.MouseAxisX, 0), Space.Self);
    }
}

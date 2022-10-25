using Framework;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform PlayerCameraTsf;
    public float Speed;
    private Transform _selfTsf;
    private float _cameraRotation;

    private void Awake()
    {
        _selfTsf = transform;
    }

    private void Start()
    {
        var inputSystem = PlayerInputSystem.Instance;
        inputSystem.SetCurrentMouseCursorLockMode(CursorLockMode.Locked);
        PlayerCameraTsf.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        HandleMovementInput();

        HandleMouseAxisInput();
    }

    private void HandleMovementInput()
    {
        var dir = Speed * Time.deltaTime * PlayerInputSystem.Instance.MovementDirection;
        _selfTsf.Translate(new Vector3(dir.x, 0, dir.y), Space.Self);
    }
    public void HandleMouseAxisInput()
    {
        _cameraRotation = Mathf.Clamp(_cameraRotation - PlayerInputSystem.Instance.MouseAxisY, -90, 90);
        PlayerCameraTsf.localRotation = Quaternion.Euler(_cameraRotation, 0, 0);

        _selfTsf.Rotate(new Vector3(0, PlayerInputSystem.Instance.MouseAxisX, 0), Space.Self);
    }
}

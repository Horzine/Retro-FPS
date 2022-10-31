using Framework;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool EnablePlayerGameplayInput;
    public PlayerHandController HandController;
    public WeaponController WeaponController;
    public Transform PlayerCameraTsf;
    public float Speed;
    private Transform _selfTsf;
    private float _cameraRotation;

    private void Awake()
    {
        _selfTsf = transform;
        EnablePlayerGameplayInput = true;
    }

    private void Start()
    {
        var inputSystem = PlayerInputSystem.Instance;
        inputSystem.SetCurrentMouseCursorLockMode(CursorLockMode.Locked);
        PlayerCameraTsf.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnEnable()
    {
        PlayerInputSystem.Instance.ReloadAction += OnInputReloadAction;
        PlayerInputSystem.Instance.SwapWeaponAction += OnInputSwapWeaponAction;
        PlayerInputSystem.Instance.FireActionDown += OnInputFireDownAction;
        PlayerInputSystem.Instance.FireActionUp += OnInputFireUpAction;
        PlayerInputSystem.Instance.FireActionPress += OnInputFirePressAction;
    }

    private void OnDisable()
    {
        PlayerInputSystem.Instance.ReloadAction -= OnInputReloadAction;
        PlayerInputSystem.Instance.SwapWeaponAction -= OnInputSwapWeaponAction;
        PlayerInputSystem.Instance.FireActionDown -= OnInputFireDownAction;
        PlayerInputSystem.Instance.FireActionUp -= OnInputFireUpAction;
        PlayerInputSystem.Instance.FireActionPress -= OnInputFirePressAction;
    }

    private void Update()
    {
        if (EnablePlayerGameplayInput)
        {
            HandleMovementInput();

            HandleMouseAxisInput();
        }
    }

    private void HandleMovementInput()
    {
        var inputDir = PlayerInputSystem.Instance.MovementDirection;
        var dir = Speed * Time.deltaTime * inputDir;
        _selfTsf.Translate(new Vector3(dir.x, 0, dir.y), Space.Self);

        WeaponController.HandleMovementInput(inputDir);
    }

    public void HandleMouseAxisInput()
    {
        var axisX = PlayerInputSystem.Instance.MouseAxisX;
        var axisY = PlayerInputSystem.Instance.MouseAxisY;
        _cameraRotation = Mathf.Clamp(_cameraRotation - axisY, -90, 90);
        PlayerCameraTsf.localRotation = Quaternion.Euler(_cameraRotation, 0, 0);

        _selfTsf.Rotate(new Vector3(0, axisX, 0), Space.Self);

        HandController.HandlePlayerInputMouseAxis(axisX, axisY);
    }

    private void OnInputReloadAction()
    {
        if (EnablePlayerGameplayInput)
        {
            WeaponController.OnInputReloadAction();
        }
    }

    private void OnInputSwapWeaponAction(bool isSwapNext)
    {
        if (EnablePlayerGameplayInput)
        {
            WeaponController.OnInputSwapWeaponAction(isSwapNext);
        }
    }

    private void OnInputFireDownAction()
    {
        if (EnablePlayerGameplayInput)
        {
            WeaponController.OnInputFireDownAction();
        }
    }

    private void OnInputFireUpAction()
    {
        if (EnablePlayerGameplayInput)
        {
            WeaponController.OnInputFireUpAction();
        }
    }

    private void OnInputFirePressAction()
    {
        if (EnablePlayerGameplayInput)
        {
            WeaponController.OnInputFirePressAction();
        }
    }
}

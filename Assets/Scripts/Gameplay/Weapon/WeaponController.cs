using Framework;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Pistol Pistol;
    private Animator m_Animator;
    private const string AnimTrigger_Weapon_Reload = "Weapon_Reload";
    private const string AnimTrigger_Weapon_Swap = "Weapon_Swap";

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerInputSystem.Instance.ReloadAction += OnInputReloadAction;
        PlayerInputSystem.Instance.SwapWeaponAction += OnInputSwapWeaponAction;
    }

    private void OnDestroy()
    {
        PlayerInputSystem.Instance.ReloadAction -= OnInputReloadAction;
        PlayerInputSystem.Instance.SwapWeaponAction -= OnInputSwapWeaponAction;
    }

    private void OnInputReloadAction()
    {
        if (!Pistol.HasSelfReloadAnim)
        {
            m_Animator.SetTrigger(AnimTrigger_Weapon_Reload);
        }
        Pistol.Reload();
    }

    private void OnInputSwapWeaponAction()
    {
        m_Animator.SetTrigger(AnimTrigger_Weapon_Swap);
    }

    private void HandleMovementInput()
    {
        var dir = PlayerInputSystem.Instance.MovementDirection;
        m_Animator.SetLayerWeight(1, Mathf.Min(dir.sqrMagnitude, 1));
    }

    private void Update()
    {
        HandleMovementInput();
    }
}

using Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IWeapon
{
    enum WeaponTypeEnum
    {
        Gun = 0,
        Melee = 1,
    }
    WeaponTypeEnum WeaponType { get; }
    bool IsSwaping { get; set; }
    void OnSwapOutEnd();
    void OnSwapIn();
    void OnSwapOut();
    void OnSwapInEnd();
}
public class WeaponController : MonoBehaviour
{
    public List<Gun> _guns;
    private IWeapon _currentWeapon;
    private List<IWeapon> _weaponList;
    private Animator m_Animator;
    private const string AnimTrigger_Weapon_Reload = "Weapon_Reload";
    private const string AnimTrigger_Weapon_Swap = "Weapon_Swap";
    public const float BasicReloadTime = 1;
    public const float BasicSwapTime = 1;
    private int _swapInTimer;
    private int _swapOutTimer;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        _weaponList = new List<IWeapon>(_guns);
        _currentWeapon = _weaponList.First();
        _currentWeapon.OnSwapIn();
        _currentWeapon.OnSwapInEnd();
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

        TimerManager.Instance.CloseTimer(_swapInTimer);
        TimerManager.Instance.CloseTimer(_swapOutTimer);
    }

    private void OnInputReloadAction()
    {
        if (_currentWeapon.WeaponType == IWeapon.WeaponTypeEnum.Gun && _currentWeapon is Gun gun)
        {
            if (gun.CanReload)
            {
                gun.BeginReload();
                if (!gun.HasSelfReloadAnim)
                {
                    m_Animator.SetTrigger(AnimTrigger_Weapon_Reload);
                }
            }
        }
    }

    private void OnInputSwapWeaponAction(bool isSwapNext)
    {
        if (_weaponList.Count > 1 && !_currentWeapon.IsSwaping)
        {
            for (int i = 0; i < _weaponList.Count; i++)
            {
                if (_weaponList[i] == _currentWeapon)
                {
                    _currentWeapon.OnSwapOut();
                    var newIndex = i + (isSwapNext ? 1 : -1);
                    IWeapon newWeapon = null;
                    if (newIndex < 0)
                    {
                        newWeapon = _weaponList.Last();
                    }
                    else if (newIndex >= _weaponList.Count)
                    {
                        newWeapon = _weaponList.First();
                    }
                    else
                    {
                        newWeapon = _weaponList[newIndex];
                    }

                    _swapOutTimer = TimerManager.Instance.Register(BasicSwapTime * 0.5f, () => { DoSwapWeapon(_currentWeapon, newWeapon); });
                    m_Animator.SetTrigger(AnimTrigger_Weapon_Swap);
                    break;
                }
            }
        }
    }

    private void DoSwapWeapon(IWeapon oldWeapon, IWeapon newWeapon)
    {
        oldWeapon.OnSwapOutEnd();
        _swapInTimer = TimerManager.Instance.Register(BasicSwapTime * 0.5f, newWeapon.OnSwapInEnd);
        newWeapon.OnSwapIn();
        _currentWeapon = newWeapon;
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

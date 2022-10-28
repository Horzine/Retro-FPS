using Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    private IWeapon _currentWeapon;
    private List<IWeapon> _weaponList;
    private Animator _animator;
    private const string AnimTrigger_Weapon_Reload = "Weapon_Reload";
    private const string AnimTrigger_Weapon_Swap = "Weapon_Swap";
    public const float BasicReloadTime = 1;
    public const float BasicSwapTime = 1;
    private int _swapInTimerId;
    private int _swapOutTimerId;
    private Transform _selfTsf;

    private void Awake()
    {
        _selfTsf = transform;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerInputSystem.Instance.ReloadAction += OnInputReloadAction;
        PlayerInputSystem.Instance.SwapWeaponAction += OnInputSwapWeaponAction;

        InitWeaponList();

        _currentWeapon = _weaponList.FirstOrDefault();
        _currentWeapon?.OnSwapIn();
        _currentWeapon?.OnSwapInEnd();
    }

    private void InitWeaponList()
    {
        _weaponList = new List<IWeapon>(WeaponSystem.Instance.TotalWeaponEnumCount);
        WeaponSystem.Instance.InitWeaponList(new List<WeaponSystem.WeaponEnum> {
            WeaponSystem.WeaponEnum.Revolver,
            WeaponSystem.WeaponEnum.AssaultRifle,
            WeaponSystem.WeaponEnum.Shotgun }
        , ref _weaponList);
        foreach (var item in _weaponList)
        {
            item?.GameObject.transform.SetParent(_selfTsf, false);
            item?.GameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        PlayerInputSystem.Instance.ReloadAction -= OnInputReloadAction;
        PlayerInputSystem.Instance.SwapWeaponAction -= OnInputSwapWeaponAction;

        TimerManager.Instance.CloseTimer(ref _swapInTimerId);
        TimerManager.Instance.CloseTimer(ref _swapOutTimerId);
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
                    _animator.SetTrigger(AnimTrigger_Weapon_Reload);
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

                    TimerManager.Instance.Register(ref _swapOutTimerId, BasicSwapTime * 0.5f, () => { DoSwapWeapon(_currentWeapon, newWeapon); });
                    _animator.SetTrigger(AnimTrigger_Weapon_Swap);

                    break;
                }
            }
        }
    }

    private void DoSwapWeapon(IWeapon oldWeapon, IWeapon newWeapon)
    {
        oldWeapon.OnSwapOutEnd();
        TimerManager.Instance.Register(ref _swapInTimerId, BasicSwapTime * 0.5f, newWeapon.OnSwapInEnd);
        newWeapon.OnSwapIn();
        _currentWeapon = newWeapon;
    }

    private void HandleMovementInput()
    {
        var dir = PlayerInputSystem.Instance.MovementDirection;
        _animator.SetLayerWeight(1, Mathf.Min(dir.sqrMagnitude, 1));
    }

    private void Update()
    {
        HandleMovementInput();
    }

}

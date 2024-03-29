using Framework;
using UnityEngine;

public class Gun : WeaponBase
{
    [SerializeField] private GunConfig _config;
    private GunData _data;
    private SpriteAnimator _spriteAniamtion;
    private float _nextEnableFireTime;
    private Camera _mainCamera;
    private int _doReloadTimerId;
    private const string FireAnimName = nameof(GunConfig.FireAnim);
    private const string IdleAnimName = nameof(GunConfig.IdleAnim);
    private const string ReloadAnimName = nameof(GunConfig.ReloadAnim);
    private const string BoltAnimName = nameof(GunConfig.BoltAnim);

    private void Awake()
    {
        _data = new();
        _data.BindConfig(_config);

        _mainCamera = Camera.main;

        _spriteAniamtion = GetComponent<SpriteAnimator>();
        var renderer = GetComponent<SpriteRenderer>();
        _spriteAniamtion.AddAnimation(FireAnimName, _config.FireAnim, renderer, OnFireAnimEndCallback);
        _spriteAniamtion.AddAnimation(IdleAnimName, _config.IdleAnim, renderer, OnFireAnimEndCallback);
        if (HasSelfReloadAnim)
        {
            _spriteAniamtion.AddAnimation(ReloadAnimName, _config.ReloadAnim, renderer, OnFireAnimEndCallback);
        }
        if (HasSelfBoltAnim)
        {
            _spriteAniamtion.AddAnimation(BoltAnimName, _config.BoltAnim, renderer, OnFireAnimEndCallback);
        }

        _nextEnableFireTime = Time.time;
    }

    private void OnEnable()
    {
        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }

    private void OnDisable()
    {
        TimerManager.Instance.CloseTimer(ref _doReloadTimerId);
    }

    public override void OnInputFireActionDown()
    {
        if (_config.FireMode == GunConfig.FireModeEnum.SemiAuto && CanFire)
        {
            DoFire();
        }
    }

    public override void OnInputFireActionUp() { }

    public override void OnInputFireActionPress()
    {
        if (_config.FireMode == GunConfig.FireModeEnum.FullAuto && CanFire)
        {
            DoFire();
        }
    }

    private void DoFire()
    {
        _data.Fire();
        FireRayCastToTarget(new AttackInfo { DamagePoint = _config.DamagePoint, MaxDistance = _config.BulletMaxDistance, MultiRayCast = false });
        _nextEnableFireTime = Time.time + _config.FireIntervalTime + (_config.NeedBolt ? _config.BoltTime : 0);
        _spriteAniamtion.PlayAnimation(FireAnimName);
    }

    private void OnFireAnimEndCallback(string animName)
    {
        switch (animName)
        {
            case FireAnimName:
                {
                    if (_config.NeedBolt && _config.HasSelfBoltAnim)
                    {
                        _spriteAniamtion.PlayAnimation(BoltAnimName);
                    }
                    else
                    {
                        _spriteAniamtion.PlayAnimation(IdleAnimName);
                    }
                    break;
                }
            case BoltAnimName:
                {
                    _spriteAniamtion.PlayAnimation(IdleAnimName);
                    break;
                }
            case IdleAnimName:
                {
                    break;
                }

            default:
                this.PrintError(nameof(OnFireAnimEndCallback), $"switch no match this name: {animName}");
                break;
        }

    }

    public void FireRayCastToTarget(AttackInfo attackInfo)
    {
        var cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (attackInfo.MultiRayCast)
        {
            var result = Physics.RaycastAll(cameraRay, attackInfo.MaxDistance);
            foreach (var item in result)
            {
                AttackInfo.SendAttackInfoToTarger(item.collider, attackInfo, item.point, item.normal);
            }
        }
        else
        {
            if (Physics.Raycast(cameraRay, out var hitInfo, attackInfo.MaxDistance))
            {
                AttackInfo.SendAttackInfoToTarger(hitInfo.collider, attackInfo, hitInfo.point, hitInfo.normal);
            }
        }
    }

    public override void OnInputReloadAction()
    {
        if (CanReload)
        {
            _data.BeginReload();
            TimerManager.Instance.Register(ref _doReloadTimerId, ReloadTime, DoReload);
            if (HasSelfReloadAnim)
            {
                _spriteAniamtion.PlayAnimation(ReloadAnimName);
            }
        }
    }

    public void DoReload()
    {
        _data.DoReloadAmmunition();
    }

    public void CancelReload()
    {
        if (_data.IsReloading)
        {
            TimerManager.Instance.CloseTimer(ref _doReloadTimerId);
            _data.CancelReload();
        }
        else
        {
            this.PrintError(nameof(CancelReload), "Why cancel reload? this gun not on reloading");
        }
    }

    public bool CanReload => _data.CanReload && !IsSwaping;

    public bool CanFire => _data.CanFire && Time.time >= _nextEnableFireTime && !IsSwaping;

    public bool HasSelfReloadAnim => _config.HasSelfReloadAnim;

    public bool HasSelfBoltAnim => _config.NeedBolt && _config.HasSelfBoltAnim;

    private float ReloadTime => WeaponController.BasicReloadTime * _config.ReloadSpeedMultiple;

    public override IWeapon.WeaponTypeEnum WeaponType => IWeapon.WeaponTypeEnum.Gun;

    public override void OnSwapOut()
    {
        base.OnSwapOut();

        if (_data.IsReloading)
        {
            CancelReload();
        }
    }

    public override void OnSwapIn()
    {
        base.OnSwapIn();

        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 0, 300, 20), $"Current Ammo: {_data.CurrentMagzineAmmo}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Backup Ammo: {_data.CurrentBackupAmmo}");
        GUI.Label(new Rect(10, 60, 300, 20), $"Can Fire: {CanFire}");
        GUI.Label(new Rect(10, 90, 300, 20), $"Can Reload: {CanReload}");
    }

    public override void OnInputSecondaryFireAction() { }
}

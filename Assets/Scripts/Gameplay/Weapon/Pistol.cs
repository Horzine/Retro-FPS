using Framework;
using System.Timers;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private WeaponConfig _config;
    private WeaponData _data;
    private SpriteAnimator _spriteAniamtion;
    private float _nextEnableFireTime;
    private Camera _mainCamera;
    private int _doReloadTimer;

    private const string FireAnimName = nameof(WeaponConfig.FireAnim);
    private const string IdleAnimName = nameof(WeaponConfig.IdleAnim);
    private const string ReloadAnimName = nameof(WeaponConfig.ReloadAnim);

    private void Awake()
    {
        _data = new();
        _data.BindConfig(_config);

        _nextEnableFireTime = Time.realtimeSinceStartup;
    }

    void Start()
    {
        _mainCamera = Camera.main;

        _spriteAniamtion = GetComponent<SpriteAnimator>();
        var renderer = GetComponent<SpriteRenderer>();
        _spriteAniamtion.AddAnimation(FireAnimName, _config.FireAnim, renderer, OnFireAnimEndCallback);
        _spriteAniamtion.AddAnimation(IdleAnimName, _config.IdleAnim, renderer, OnFireAnimEndCallback);
        if (HasSelfReloadAnim)
        {
            _spriteAniamtion.AddAnimation(ReloadAnimName, _config.ReloadAnim, renderer, OnFireAnimEndCallback);
        }

        PlayerInputSystem.Instance.FireAction += OnInputFireAction;

        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }

    private void OnDestroy()
    {
        PlayerInputSystem.Instance.FireAction -= OnInputFireAction;
        TimerManager.Instance.CloseTimer(_doReloadTimer);
    }

    private void OnInputFireAction()
    {
        if (CanFire)
        {
            _data.Fire();
            FireRayCastToTarget(new AttackInfo { DamagePoint = _config.DamagePoint, MaxDistance = _config.BulletMaxDistance, MultiRayCast = false });
            _nextEnableFireTime = Time.realtimeSinceStartup + _config.FireIntervalTime;
            _spriteAniamtion.PlayAnimation(FireAnimName);
        }
    }

    private void OnFireAnimEndCallback(string _)
    {
        _spriteAniamtion.PlayAnimation(IdleAnimName);
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

    public void BeginReload()
    {
        if (CanReload)
        {
            _data.BeginReload();
            _doReloadTimer = TimerManager.Instance.Register(ReloadTime, DoReload);
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

    public bool CanReload => _data.CanReload;

    public bool HasSelfReloadAnim => _config.HasSelfReloadAnim;

    public bool CanFire => _data.CanFire && Time.realtimeSinceStartup >= _nextEnableFireTime;

    private float ReloadTime => WeaponController.BasicReloadTime * _config.ReloadSpeedMultiple;


    private void OnGUI()
    {
        GUI.Label(new Rect(10, 0, 300, 20), $"Current Ammo: {_data.CurrentMagzineAmmo}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Backup Ammo: {_data.CurrentBackupAmmo}");
        GUI.Label(new Rect(10, 60, 300, 20), $"Can Fire: {CanFire}");
        GUI.Label(new Rect(10, 90, 300, 20), $"Can Reload: {CanReload}");
    }
}

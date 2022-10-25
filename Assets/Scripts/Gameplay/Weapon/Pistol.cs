using Framework;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private WeaponData _data;
    private SpriteAnimator _spriteAniamtion;
    private float _nextEnableFireTime;
    private float _nextEnableReloadTime;
    private Camera _mainCamera;

    private const string FireAnimName = nameof(WeaponData.FireAnim);
    private const string IdleAnimName = nameof(WeaponData.IdleAnim);
    private const string ReloadAnimName = nameof(WeaponData.ReloadAnim);


    private void Awake()
    {
        _nextEnableFireTime = Time.realtimeSinceStartup;
        _nextEnableReloadTime = Time.realtimeSinceStartup;
    }

    void Start()
    {
        _mainCamera = Camera.main;

        _spriteAniamtion = GetComponent<SpriteAnimator>();
        var renderer = GetComponent<SpriteRenderer>();
        _spriteAniamtion.AddAnimation(FireAnimName, _data.FireAnim, renderer, OnFireAnimEndCallback);
        _spriteAniamtion.AddAnimation(IdleAnimName, _data.IdleAnim, renderer, OnFireAnimEndCallback);
        if (HasSelfReloadAnim)
        {
            _spriteAniamtion.AddAnimation(ReloadAnimName, _data.ReloadAnim, renderer, OnFireAnimEndCallback);
        }

        PlayerInputSystem.Instance.FireAction += OnInputFireAction;

        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }
    private void OnDestroy()
    {
        PlayerInputSystem.Instance.FireAction -= OnInputFireAction;
    }

    private void OnInputFireAction()
    {
        if (CanFire)
        {
            _data.Fire();
            FireRayCastToTarget(new AttackInfo { DamagePoint = _data.DamagePoint, MaxDistance = _data.BulletMaxDistance, MultiRayCast = false });
            _nextEnableFireTime = Time.realtimeSinceStartup + _data.FireIntervalTime;
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

    public void Reload()
    {
        _data.ReloadAmmunition();
        _nextEnableReloadTime = Time.realtimeSinceStartup + WeaponController.BasicReloadTime * _data.ReloadSpeedMultiple;
        if (HasSelfReloadAnim)
        {
            _spriteAniamtion.PlayAnimation(ReloadAnimName);
        }
    }

    public bool CanReload => _data.CanReload && Time.realtimeSinceStartup >= _nextEnableReloadTime;

    public bool HasSelfReloadAnim => _data.HasSelfReloadAnim;

    public bool CanFire => _data.CanFire && Time.realtimeSinceStartup >= _nextEnableFireTime;
}

using Framework;
using System;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    private SpriteAnimator _spriteAniamtion;
    public Sprite[] FireAnim;
    public Sprite[] IdleAnim;
    public Sprite[] ReloadAnim;
    public float RoundsPerMinute = 1;
    private float _fireIntervalTime;
    private float _nextEnableFireTime;

    private const string FireAnimName = nameof(FireAnim);
    private const string IdleAnimName = nameof(IdleAnim);
    private const string ReloadAnimName = nameof(ReloadAnim);

    private Camera mainCamera;

    private void Awake()
    {
        _fireIntervalTime = 60 / RoundsPerMinute;
        _nextEnableFireTime = Time.realtimeSinceStartup;
    }

    void Start()
    {
        mainCamera = Camera.main;

        _spriteAniamtion = GetComponent<SpriteAnimator>();
        var renderer = GetComponent<SpriteRenderer>();
        _spriteAniamtion.AddAnimation(FireAnimName, FireAnim, renderer, OnFireAnimEndCallback);
        _spriteAniamtion.AddAnimation(IdleAnimName, IdleAnim, renderer, OnFireAnimEndCallback);
        _spriteAniamtion.AddAnimation(ReloadAnimName, ReloadAnim, renderer, OnFireAnimEndCallback);

        PlayerInputSystem.Instance.FireAction += OnInputFireAction;

        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }
    private void OnDestroy()
    {
        PlayerInputSystem.Instance.FireAction -= OnInputFireAction;
    }

    private void OnInputFireAction()
    {
        if (Time.realtimeSinceStartup >= _nextEnableFireTime)
        {
            FireRayCastToTarget(new AttackInfo { DamagePoint = 100, MaxDistance = 10, MultiRayCast = false });
            _nextEnableFireTime = Time.realtimeSinceStartup + _fireIntervalTime;
            _spriteAniamtion.PlayAnimation(FireAnimName);
        }
    }

    private void OnFireAnimEndCallback(string _)
    {
        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }

    public void FireRayCastToTarget(AttackInfo attackInfo)
    {
        var cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
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
        if (HasSelfReloadAnim())
        {
            _spriteAniamtion.PlayAnimation(ReloadAnimName);
        }
    }

    public bool HasSelfReloadAnim()
    {
        return ReloadAnim != null && ReloadAnim.Length > 0;
    }
}

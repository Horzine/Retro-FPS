using Framework;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public PlayerInputSystem InputSystem;
    private SpriteAnimator _spriteAniamtion;
    public Sprite[] FireAnim;
    public Sprite[] IdleAnim;
    public float RoundsPerMinute = 1;
    private float _fireIntervalTime;
    private float _nextEnableFireTime;

    private const string FireAnimName = nameof(FireAnim);
    private const string IdleAnimName = nameof(IdleAnim);

    private void Awake()
    {
        _fireIntervalTime = 60 / RoundsPerMinute;
        _nextEnableFireTime = Time.realtimeSinceStartup;
    }

    void Start()
    {
        _spriteAniamtion = GetComponent<SpriteAnimator>();
        var render = GetComponent<SpriteRenderer>();
        _spriteAniamtion.AddAnimation(FireAnimName, FireAnim, render, OnFireAnimEndCallback);
        _spriteAniamtion.AddAnimation(IdleAnimName, IdleAnim, render, OnFireAnimEndCallback);

        PlayerInputSystem.Instance.FireAction += OnInputFireAction;

        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }
    private void OnDestroy()
    {
        PlayerInputSystem.Instance.FireAction -= OnInputFireAction;
    }

    private void OnInputFireAction()
    {
        TryFireBullet();
        _spriteAniamtion.PlayAnimation(FireAnimName);
    }

    private void OnFireAnimEndCallback(string _)
    {
        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }

    private void TryFireBullet()
    {
        if (Time.realtimeSinceStartup >= _nextEnableFireTime)
        {
            FireRayCastToTarget(new AttackInfo { DamagePoint = 100, MaxDistance = 10, MultiRayCast = false });
            _nextEnableFireTime = Time.realtimeSinceStartup + _fireIntervalTime;
        }
    }

    public void FireRayCastToTarget(AttackInfo attackInfo)
    {
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
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
}

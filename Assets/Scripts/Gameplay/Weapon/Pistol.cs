using Framework;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public PlayerInputSystem InputSystem;
    private SpriteAnimator _spriteAniamtion;
    public Sprite[] FireAnim;
    public Sprite[] IdleAnim;

    private const string FireAnimName = nameof(FireAnim);
    private const string IdleAnimName = nameof(IdleAnim);

    void Start()
    {
        _spriteAniamtion = GetComponent<SpriteAnimator>();
        var render = GetComponent<SpriteRenderer>();
        _spriteAniamtion.AddAnimation(FireAnimName, FireAnim, render, OnFireAnimEndCallback);
        _spriteAniamtion.AddAnimation(IdleAnimName, IdleAnim, render, OnFireAnimEndCallback);

        InputSystem.FireAction += OnInputFireAction;

        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }

    private void OnInputFireAction()
    {
        _spriteAniamtion.PlayAnimation(FireAnimName);
    }

    private void OnFireAnimEndCallback(string _)
    {
        _spriteAniamtion.PlayAnimation(IdleAnimName);
    }


}

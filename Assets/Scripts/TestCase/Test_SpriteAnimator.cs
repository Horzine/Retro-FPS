using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

/*
  ┎━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┒
  ┃   Dedication Focus Discipline   ┃
  ┃        Practice more !!!        ┃
  ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
namespace TestCase
{
    public class Test_SpriteAnimator : MonoBehaviour
    {
        public Sprite[] idle;
        public Sprite[] walk;
        public Sprite[] attack;
        public Sprite[] death;
        public Sprite[] hit;

        private SpriteAnimator _animator;
        private SpriteRenderer _render;

        private void Start()
        {
            _animator = GetComponent<SpriteAnimator>();
            _render = GetComponent<SpriteRenderer>();

            _animator.AddAnimation(nameof(idle), idle, _render, OnAnimationEndCallback, true);
            _animator.AddAnimation(nameof(walk), walk, _render, OnAnimationEndCallback, true, 0.05f);
            _animator.AddAnimation(nameof(attack), attack, _render, OnAnimationEndCallback,
                SpriteAnimator.DefaultLoop, SpriteAnimator.DefaultIntervalTime, AttackAnimationEvent, 10);
            _animator.AddAnimation(nameof(death), death, _render, OnAnimationEndCallback);
            _animator.AddAnimation(nameof(hit), hit, _render, OnAnimationEndCallback);

            _animator.PlayAnimation(nameof(idle));
        }

        private void OnAnimationEndCallback(string animName)
        {
            switch (animName)
            {
                case nameof(idle):
                    {
                        Debug.Log($"idle anim end");
                        break;
                    }
                case nameof(walk):
                    {
                        Debug.Log($"walk anim end");
                        break;
                    }
                case nameof(attack):
                    {
                        Debug.Log($"attack anim end");
                        break;
                    }
                case nameof(death):
                    {
                        Debug.Log($"death anim end");
                        break;
                    }
                case nameof(hit):
                    {
                        Debug.Log($"hit anim end");
                        _animator.PlayAnimation(nameof(idle));
                        break;
                    }
                default:
                    throw new System.Exception("Unknown animation name OnAnimationEndCallback");
            }
        }

        private void AttackAnimationEvent()
        {
            Debug.Log("Attack anim event");
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, 50), nameof(idle)))
            {
                _animator.PlayAnimation(nameof(idle));
            }

            if (GUI.Button(new Rect(0, 50, 100, 50), nameof(walk)))
            {
                _animator.PlayAnimation(nameof(walk));
            }

            if (GUI.Button(new Rect(0, 100, 100, 50), nameof(attack)))
            {
                _animator.PlayAnimation(nameof(attack));
            }

            if (GUI.Button(new Rect(0, 150, 100, 50), nameof(death)))
            {
                _animator.PlayAnimation(nameof(death));
            }

            if (GUI.Button(new Rect(0, 200, 100, 50), nameof(hit)))
            {
                _animator.PlayAnimation(nameof(hit));
            }
        }
    }
}
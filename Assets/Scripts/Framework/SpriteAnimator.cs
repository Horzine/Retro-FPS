using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  ┎━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┒
  ┃   Dedication Focus Discipline   ┃
  ┃        Practice more !!!        ┃
  ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
namespace Framework
{
    public class SpriteAnimator : MonoBehaviour
    {
        public const float DefaultIntervalTime = 0.03f;
        public const bool DefaultLoop = false;

        private Dictionary<string, SpriteAnimation> _allAnimations = new Dictionary<string, SpriteAnimation>();
        private Coroutine _currentAnimCoroutine;

        public void AddAnimation(string name,
            Sprite[] sprites,
            SpriteRenderer render,
            Action<string> animationEndCallback,
            bool loop = DefaultLoop,
            float intervalTime = DefaultIntervalTime,
            Action animationEvent = null,
            int animationEventIndex = 0)
        {
            if (!_allAnimations.ContainsKey(name))
            {
                var anim = new SpriteAnimation(name,
                    sprites,
                    render,
                    animationEndCallback,
                    loop,
                    intervalTime,
                    animationEvent,
                    animationEventIndex);
                _allAnimations.Add(name, anim);
            }
            else
            {
                throw new Exception($"All animations already has this name: {name}");
            }
        }

        public void PlayAnimation(string name)
        {
            if (_allAnimations.ContainsKey(name))
            {
                if (_currentAnimCoroutine != null)
                {
                    StopCoroutine(_currentAnimCoroutine);
                }
                var anim = _allAnimations[name];
                _currentAnimCoroutine = StartCoroutine(anim.GetEnumerator());
            }
            else
            {
                throw new Exception($"Animation name is invalid: {name}");
            }
        }

        public void SetAnimIntervalTime(string name, float intervalTime)
        {
            if (_allAnimations.ContainsKey(name))
            {
                var anim = _allAnimations[name];
                anim.IntervalTime = intervalTime;
            }
            else
            {
                throw new Exception($"Animation name is invalid: {name}");
            }
        }
    }
}
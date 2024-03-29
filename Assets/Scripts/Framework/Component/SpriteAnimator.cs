﻿using System;
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

        private Dictionary<string, SpriteAnimation> _allAnimations = new();
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
            if (sprites == null || sprites.Length == 0)
            {
                return;
            }


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
                throw new Exception($"AllAnimations already has this name: {name}");
            }
        }

        public void PlayAnimation(string name)
        {
            if (_allAnimations.TryGetValue(name, out var anim))
            {
                if (_currentAnimCoroutine != null)
                {
                    StopCoroutine(_currentAnimCoroutine);
                }
                _currentAnimCoroutine = StartCoroutine(anim.GetEnumerator());
            }
            else
            {
                throw new Exception($"Animation name is invalid: {name}");
            }
        }

        public void SetAnimIntervalTime(string name, float intervalTime)
        {
            if (_allAnimations.TryGetValue(name, out var anim))
            {
                anim.IntervalTime = intervalTime;
            }
            else
            {
                throw new Exception($"Animation name is invalid: {name}");
            }
        }
    }
}
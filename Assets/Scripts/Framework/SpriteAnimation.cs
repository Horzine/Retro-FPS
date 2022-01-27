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
    public class SpriteAnimation : IEnumerable
    {
        private string _name;
        private Sprite[] _sprites;
        private SpriteRenderer _render;
        private bool _loop;
        private int _animationEventIndex;
        private Action _animationEvent;
        private Action<string> _animationEndCallback;
        public float IntervalTime { get; set; }

        public SpriteAnimation(string name,
            Sprite[] sprites,
            SpriteRenderer render,
            Action<string> animationEndCallback,
            bool loop,
            float intervalTime,
            Action animationEvent,
            int animationEventIndex)
        {
            _name = name;
            _sprites = sprites;
            _render = render;
            _animationEndCallback = animationEndCallback;
            _loop = loop;
            IntervalTime = intervalTime;
            _animationEvent = animationEvent;
            _animationEventIndex = animationEventIndex;
        }

        public IEnumerator GetEnumerator()
        {
            var waitSeconds = new WaitForSeconds(IntervalTime);
            int index = 0;
            while (index < _sprites.Length)
            {
                _render.sprite = _sprites[index];
                if (index == _animationEventIndex)
                {
                    _animationEvent?.Invoke();
                }
                ++index;
                yield return waitSeconds;
                if (index >= _sprites.Length && _loop)
                {
                    index = 0;
                }
            }
            _animationEndCallback?.Invoke(_name);
        }

    }
}
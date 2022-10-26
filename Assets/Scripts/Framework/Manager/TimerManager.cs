using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TimerManager : MonoSingleton<TimerManager>
    {
        private int _idIndex = 0;
        private readonly List<Timer> _timerWillAdd = new();
        private readonly List<int> _timerWillRemove = new();
        private readonly List<(int timerId, float interval, bool loop)> _timerWillModify = new();
        private readonly SortedList<int, Timer> timers = new();

        // TODO; void RegisterNoLoop
        public int Register(float interval, Action triggerCallback, bool loop = false)
        {
            var timer = CreateTimer();
            timer.Loop = loop;
            timer.Interval = interval;
            timer.TriggerCallback += triggerCallback;
            _timerWillAdd.Add(timer);
            return timer.TimerId;
        }

        public void ResetTimerTime(int timerId)
        {
            GetTimer(timerId)?.ResetTime();
        }

        public void ForceTriggerTimer(int timerId)
        {
            GetTimer(timerId)?.ForceTrigger();
        }

        public void CloseTimer(int timerId, bool ShouldTrigger = false)
        {
            var timer = GetTimer(timerId);
            if (ShouldTrigger)
            {
                timer?.InvokeTriggerCallback();
            }
            timer?.InvokeFinishCallback();
        }

        public void SetTimerLoop(int timerId, bool loop)
        {
            var timer = GetTimer(timerId);
            if (timer != null)
            {
                _timerWillModify.Add((timerId, timer.Interval, loop));
            }
        }

        public void SetTimerInterval(int timerId, float interval)
        {
            var timer = GetTimer(timerId);
            if (timer != null)
            {
                _timerWillModify.Add((timerId, interval, timer.Loop));
            }
        }


        private Timer CreateTimer()
        {
            return new Timer(_idIndex++, OnTimerFinishAction);
        }

        private void OnTimerFinishAction(int timerId)
        {
            _timerWillRemove.Add(timerId);
        }

        private void HandleTimerWillAdd()
        {
            if (_timerWillAdd.Count > 0)
            {
                foreach (var item in _timerWillAdd)
                {
                    timers.Add(item.TimerId, item);
                }
                _timerWillAdd.Clear();
            }
        }

        private void HandleTimeWillRemove()
        {
            if (_timerWillRemove.Count > 0)
            {
                foreach (var item in _timerWillRemove)
                {
                    timers.Remove(item);
                }
                _timerWillRemove.Clear();
            }
        }

        private void HandleTimerWillModify()
        {
            if (_timerWillModify.Count > 0)
            {
                foreach (var (timerId, interval, loop) in _timerWillModify)
                {
                    var timer = GetTimer(timerId);
                    if (timer != null)
                    {
                        timer.Interval = interval;
                        timer.Loop = loop;
                    }
                }
                _timerWillModify.Clear();
            }
        }

        private void Update()
        {
            HandleTimerWillAdd();

            HandleTimerWillModify();

            HandleTimeWillRemove();

            UpdateAllTimers();
        }

        private void UpdateAllTimers()
        {
            foreach (var item in timers.Keys)
            {
                var timer = GetTimer(item);
                timer.OnUpdate(Time.deltaTime);
            }
        }

        private Timer GetTimer(int timerId)
        {
            if (!timers.TryGetValue(timerId, out var Timer))
            {
                this.PrintError(nameof(GetTimer), $"timerId :{timerId} not contain in SortedList");
            }
            return Timer;
        }


        private class Timer
        {
            public Timer(int timerId, Action<int> finishAction)
            {
                TimerId = timerId;
                _finishAction = finishAction;
            }
            ~Timer()
            {
                Debug.Log($"~Timer id: {TimerId}");
            }
            private float _time;
            private event Action<int> _finishAction;
            public int TimerId { get; set; }
            public bool Loop { get; set; }
            public float Interval { get; set; }
            public event Action TriggerCallback;

            public void OnUpdate(float deltaTime)
            {
                _time += deltaTime;
                if (_time >= Interval)
                {
                    OnTrigger();
                }
            }

            private void OnTrigger()
            {
                InvokeTriggerCallback();

                if (Loop)
                {
                    _time = Mathf.Max(0, _time - Interval);
                }
                else
                {
                    InvokeFinishCallback();
                }
            }

            public void ResetTime()
            {
                _time = 0;
            }

            public void ForceTrigger()
            {
                OnTrigger();
            }

            public void InvokeTriggerCallback()
            {
                TriggerCallback?.Invoke();
            }

            public void InvokeFinishCallback()
            {
                _finishAction?.Invoke(TimerId);
            }

        }
    }
}

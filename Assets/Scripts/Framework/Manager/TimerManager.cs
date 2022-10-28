using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TimerManager : MonoSingleton<TimerManager>
    {
        private int _idIndex = 1;
        private readonly List<Timer> _timerWillAdd = new();
        private readonly List<int> _timerWillRemove = new();
        private readonly List<(int timerId, float interval)> _timerWillModifyInterval = new();
        private readonly List<(int timerId, bool loop)> _timerWillModifyLoop = new();
        private readonly SortedList<int, Timer> timers = new();

        public void Register(ref int timerId, float interval, Action triggerCallback, bool loop = false)
        {
            var timer = CreateTimer();
            timer.Loop = loop;
            timer.ModifyInterval(interval);
            timer.TriggerCallback += triggerCallback;
            _timerWillAdd.Add(timer);
            timerId = timer.TimerId;
        }

        public void ResetTimerTime(int timerId)
        {
            GetTimer(timerId)?.ResetNextInvokeTime();
        }

        public void ForceTriggerTimer(int timerId)
        {
            GetTimer(timerId)?.ForceTrigger();
        }

        public void CloseTimer(ref int timerId, bool ShouldTrigger = false)
        {
            var timer = GetTimer(timerId);
            if (ShouldTrigger)
            {
                timer?.InvokeTriggerCallback();
            }
            timer?.InvokeFinishCallback();
            timerId = default;
        }

        public void SetTimerLoop(int timerId, bool loop)
        {
            var timer = GetTimer(timerId);
            if (timer != null)
            {
                _timerWillModifyLoop.Add((timerId, loop));
            }
        }

        public void SetTimerInterval(int timerId, float interval)
        {
            var timer = GetTimer(timerId);
            if (timer != null)
            {
                _timerWillModifyInterval.Add((timerId, interval));
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
            if (_timerWillModifyLoop.Count > 0)
            {
                foreach (var (timerId, loop) in _timerWillModifyLoop)
                {
                    var timer = GetTimer(timerId);
                    if (timer != null)
                    {
                        timer.Loop = loop;
                    }
                }
                _timerWillModifyLoop.Clear();
            }

            if (_timerWillModifyInterval.Count > 0)
            {
                foreach (var (timerId, interval) in _timerWillModifyInterval)
                {
                    GetTimer(timerId)?.ModifyInterval(interval);
                }
                _timerWillModifyInterval.Clear();
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
                this.PrintWarning(nameof(GetTimer), $"timerId :{timerId} not contain in SortedList");
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
                // Debug.Log($"Timer: {TimerId} has been released");
            }
            private float _time;
            private float _nextInvokeTime;
            private event Action<int> _finishAction;
            public int TimerId { get; set; }
            public bool Loop { get; set; }
            public float Interval { get; private set; }
            public event Action TriggerCallback;

            public void OnUpdate(float deltaTime)
            {
                _time += deltaTime;
                if (_time >= _nextInvokeTime)
                {
                    OnTrigger();
                }
            }

            private void OnTrigger()
            {
                InvokeTriggerCallback();

                if (Loop)
                {
                    _nextInvokeTime += Interval;
                }
                else
                {
                    InvokeFinishCallback();
                }
            }

            public void ResetNextInvokeTime()
            {
                _nextInvokeTime = _time + Interval;
            }

            public void ForceTrigger()
            {
                OnTrigger();
                ResetNextInvokeTime();
            }

            public void InvokeTriggerCallback()
            {
                TriggerCallback?.Invoke();
                // Debug.Log($"{Time.time} Timer: InvokeTriggerCallback");
            }

            public void InvokeFinishCallback()
            {
                _finishAction?.Invoke(TimerId);
            }

            public void ModifyInterval(float newInterval)
            {// notice: ModifyInterval will ResetNextInvokeTime
                Interval = newInterval;
                ResetNextInvokeTime();
            }
        }
    }
}

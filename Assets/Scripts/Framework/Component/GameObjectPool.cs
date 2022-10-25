using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public interface IGameObjectPoolEntry
{
    GameObject GameObject { get; }
    float AutoRecoverTime { get; }
    bool AutoRecover { get; }
    void Reset();
    void OnDeactivate();
    void OnActivate();
}
public class GameObjectPool<T> where T : Component, IGameObjectPoolEntry
{
    private readonly Queue<T> _waitingQueue = new(MaxPoolCount);
    private readonly Queue<T> _workingQueue = new(MaxPoolCount);
    private Timer _recoverTimer;
    private const int MaxPoolCount = 10;
    private float _recoverTime = 5;
    public event Action<T> InitCallback;


    public void Init()
    {
        _recoverTimer = new Timer
        {
            AutoReset = true,
            Interval = _recoverTime * 1000,
        };
        _recoverTimer.Elapsed += (_, _) => Recover();
        _recoverTimer.Start();
    }

    public void OnDestory()
    {
        _recoverTimer?.Dispose();
    }

    public T GetObject()
    {
        var entry = WaitingTransitionWorking();
        if (!entry)
        {
            if (_workingQueue.Count >= MaxPoolCount)
            {
                WorkingTransitionWaiting();
            }
            else
            {
                entry = new GameObject(typeof(T).Name).AddComponent<T>();
                InitCallback?.Invoke(entry);
                _waitingQueue.Enqueue(entry);
            }
            entry = WaitingTransitionWorking();
        }

        return entry;
    }


    private T WaitingTransitionWorking()
    {
        if (_waitingQueue.TryDequeue(out var entry))
        {
            _workingQueue.Enqueue(entry);
            entry.OnActivate();
            Debug.Log($"Waiting To Working: _waitingQueue {_waitingQueue.Count}, _workingQueue {_workingQueue.Count}");
        }
        return entry;
    }

    private T WorkingTransitionWaiting()
    {
        if (_workingQueue.TryDequeue(out var entry))
        {
            _waitingQueue.Enqueue(entry);
            entry.OnDeactivate();
            Debug.Log($"Working To Waiting: _waitingQueue {_waitingQueue.Count}, _workingQueue {_workingQueue.Count}");
        }
        return entry;
    }

    public void Recover()
    {/// Temp for testing
        WorkingTransitionWaiting();
    }

}

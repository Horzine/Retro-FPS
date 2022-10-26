using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IGameObjectPoolEntry
{
    GameObject GameObject { get; }
    void Reset();
    void OnDeactivate();
    void OnActivate();
}
public class GameObjectPool<T> where T : Component, IGameObjectPoolEntry
{
    private readonly Queue<T> _waitingQueue = new(MaxPoolCount);
    private readonly Queue<T> _workingQueue = new(MaxPoolCount);
    private readonly int _recoverTimer = -1;
    private const int MaxPoolCount = 20;
    private readonly float _recoverTime = 3;
    public event Action<T> InitCallback;
    private readonly T _templete;

    public GameObjectPool(T templete)
    {
        _templete = templete;
        _recoverTimer = TimerManager.Instance.Register(_recoverTime, DoAutoRecover, true);
    }

    public void OnDestory()
    {
        TimerManager.Instance.CloseTimer(_recoverTimer);
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
                // entry = new GameObject(typeof(T).Name).AddComponent<T>();
                entry = Object.Instantiate(_templete);
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
            // Debug.Log($"Waiting To Working: _waitingQueue {_waitingQueue.Count}, _workingQueue {_workingQueue.Count}");
        }
        return entry;
    }

    private T WorkingTransitionWaiting()
    {
        if (_workingQueue.TryDequeue(out var entry))
        {
            _waitingQueue.Enqueue(entry);
            entry.OnDeactivate();
            // Debug.Log($"Working To Waiting: _waitingQueue {_waitingQueue.Count}, _workingQueue {_workingQueue.Count}");
        }
        return entry;
    }

    private void DoAutoRecover()
    {
        WorkingTransitionWaiting();
    }

}

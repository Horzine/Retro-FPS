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
    private const int DefaultMaxPoolCount = 20;
    private readonly Queue<T> _waitingQueue;
    private readonly Queue<T> _workingQueue;
    private readonly int _recoverTimer = -1;
    private readonly int _maxPoolCount;
    private readonly float _recoverTime = 3;
    private readonly bool _autoDoRecover;
    private readonly T _templete;
    private readonly Transform _fatherTsf;
    public event Action<T> InitCallback;

    public GameObjectPool(T templete, Transform fatherTsf, bool autoDoRecover = true, int maxPoolCount = DefaultMaxPoolCount)
    {
        _templete = templete;
        _fatherTsf = fatherTsf;
        _autoDoRecover = autoDoRecover;
        _maxPoolCount = maxPoolCount;
        _workingQueue = new(_maxPoolCount);
        _waitingQueue = new(_maxPoolCount);
        if (_autoDoRecover)
        {
            _recoverTimer = TimerManager.Instance.Register(_recoverTime, DoAutoRecover, true);
        }
    }

    public void OnDestory()
    {
        if (_autoDoRecover)
        {
            TimerManager.Instance.CloseTimer(_recoverTimer);
        }
    }

    public T GetObject()
    {
        var entry = WaitingTransitionWorking();
        if (!entry)
        {
            if (_workingQueue.Count >= _maxPoolCount)
            {
                WorkingTransitionWaiting();
            }
            else
            {
                // entry = new GameObject(typeof(T).Name).AddComponent<T>();
                entry = Object.Instantiate(_templete, _fatherTsf);
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

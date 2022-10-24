using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _isDestory;

    public static T Instance
    {
        get
        {
            if (!_instance && !_isDestory)
            {
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance)
        {
            Destroy(this);
        }
    }

    protected virtual void OnDestory()
    {
        _instance = null;
        _isDestory = true;
    }

}

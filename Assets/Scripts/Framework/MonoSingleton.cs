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
                _instance = (T)FindObjectOfType(typeof(T));
                if (!_instance)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = GetComponent<T>();
            DontDestroyOnLoad(_instance);
        }
    }

    protected virtual void OnDestory()
    {
        _instance = null;
        _isDestory = true;
    }

}

using UnityEngine;

namespace Framework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance => GetInstance();
        private static T GetInstance()
        {
            if (!_instance)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (!_instance)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                DontDestroyOnLoad(_instance);
            }
            return _instance;
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
    }

}
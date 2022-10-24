using System.Collections.Generic;
using UnityEngine;

namespace Temp
{
    public class MonoSingletonStat
    {
        public delegate void DestroyDelegate();
        public static HashSet<DestroyDelegate> DestroyInstanceDelegate = new HashSet<DestroyDelegate>();
    }

    /// <summary>
    ///     ����̳�������MonoBehavrour��ĵ���ʵ�֣����ֵ���ʵ�������ڼ��ٶԳ������Ĳ�ѯ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TempMonoSingleton<T> : MonoBehaviour where T : Component
    {
        public static string MonoSingletonRoot = "MonoSingletonRoot";
        // ��������ʵ��
        private static T _instance;

        // �ڵ����У�ÿ�������destroyed��־�����Ӧ�÷ָ��ڲ�ͬ�Ĵ洢���ռ��У���ˣ�����R#�������ʾ
        // ReSharper disable once StaticFieldInGenericType
        private static bool _destroyed;

        public static bool IsValidate()
        {
            return _instance != null;
        }


        public static T Instance
        {
            get { return GetInstance(); }
        }
        /// <summary>
        ///     ��õ���ʵ������ѯ�������Ƿ��и������ͣ�����д洢��̬���������û�У�����һ���������component��gameobject
        ///     ���ֵ���ʵ����GameObjectֱ�ӹҽ���bootroot�ڵ��£��ڳ����е��������ں���Ϸ����������ͬ�������������ʵ����ģ��
        ///     ����ͨ��DestroyInstance���й���������������
        /// </summary>
        /// <returns>���ص���ʵ��</returns>
        public static T GetInstance()
        {
            if (_instance == null && !_destroyed)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);

                    _instance = go.AddComponent<T>();
                    if (GameObject.Find(MonoSingletonRoot) != null)
                    {
                        go.transform.parent = GameObject.Find(MonoSingletonRoot).transform;
                    }

                    if (Application.isPlaying) // ��ֹ�༭����ʹ�ó���
                    {
                        DontDestroyOnLoad(go);
                    }

                }
            }

            return _instance;
        }

        /// <summary>
        ///     ɾ������ʵ��,���ּ̳й�ϵ�ĵ�����������Ӧ����ģ����ʾ����
        /// </summary>
        public static void DestroyInstance()
        {
            if (_instance != null)
                Destroy(_instance.gameObject);

            _instance = null;
            _destroyed = true;

        }

        public static void ClearDestroy()
        {
            DestroyInstance();
            _destroyed = false;
        }

        /// <summary>
        ///     Awake��Ϣ��ȷ������ʵ����Ψһ��, ɾ���´�����instance, Ҫȷ�����಻Ҫʹ��Awake
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = GetComponent<T>();
                DontDestroyOnLoad(gameObject);
            }

            MonoSingletonStat.DestroyInstanceDelegate.Add(DestroyInstance);
        }

        /// <summary>
        ///     OnDestroy��Ϣ��ȷ�������ľ�̬ʵ��������GameObject����
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_instance != null && _instance.gameObject == gameObject) _instance = null;
            _destroyed = true;

        }
    }

}
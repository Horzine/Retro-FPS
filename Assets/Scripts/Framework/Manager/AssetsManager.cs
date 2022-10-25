using System;
using System.Collections;
using UnityEngine;

namespace Framework
{
    public class AssetsManager : MonoSingleton<AssetsManager>
    {
        public T LoadComponent<T>(string path) where T : MonoBehaviour
        {
            var assetName = path.Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
            return Resources.Load<T>(assetName);
        }

        public void LoadComponentAsync<T>(string path, Action<T> callback) where T : MonoBehaviour
        {
            var assetName = path.Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
            StartCoroutine(DoLoadAssetsAsync(assetName, callback));
        }

        private IEnumerator DoLoadAssetsAsync<T>(string name, Action<T> callback) where T : MonoBehaviour
        {
            var rr = Resources.LoadAsync<T>(name);
            rr.completed += (ao) =>
            callback?.Invoke((T)rr.asset);
            while (!rr.isDone)
            {
                yield return null;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetsManager : MonoSingleton<AssetsManager>
{
    public T LoadAssets<T>(string path) where T : Object
    {
    var assetName = path.Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
        return Resources.Load<T>(assetName);
    }

    public void LoadAssetsAsync<T>(string path, Action<T> callback) where T : Object
    {
        var assetName = path.Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
        StartCoroutine(DoLoadAssetsAsync(assetName, callback));
    }

    private IEnumerator DoLoadAssetsAsync<T>(string name, Action<T> callback) where T : Object
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

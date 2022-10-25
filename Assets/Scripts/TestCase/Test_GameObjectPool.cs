using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GameObjectPool : MonoBehaviour
{
    private GameObjectPool<Test_GameObjectPoolEntry> pool = new();

    public void Awake()
    {
        pool.Init();
    }
    public void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "11111111111"))
        {
            var go = pool.GetObject();
            // this.PrintLog("OnGUI", go.name);
        }
        if (GUI.Button(new Rect(0, 100, 100, 50), "11111111111"))
        {
            pool.Recover();
        }


    }

    public void OnDestroy()
    {
        pool.OnDestory();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GameObjectPoolEntry : MonoBehaviour, IGameObjectPoolEntry
{
    public GameObject GameObject => throw new System.NotImplementedException();

    public float AutoRecoverTime => throw new System.NotImplementedException();

    public bool AutoRecover => throw new System.NotImplementedException();

    public void OnActivate()
    {
        gameObject.SetActive(true);
    }

    public void OnDeactivate()
    {
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        // throw new System.NotImplementedException();
    }
}

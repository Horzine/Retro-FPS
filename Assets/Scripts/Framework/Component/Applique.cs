using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Applique : MonoBehaviour, IGameObjectPoolEntry
{
    public GameObject GameObject => gameObject;

    public float AutoRecoverTime => throw new System.NotImplementedException();

    public bool AutoRecover => throw new System.NotImplementedException();

    public void OnActivate()
    {
        this.SetGameObjectActive(true);
    }

    public void OnDeactivate()
    {
        this.SetGameObjectActive(false);
    }

    public void Reset()
    {

    }
}

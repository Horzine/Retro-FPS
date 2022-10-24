using System;
using System.Collections.Generic;
using UnityEngine;

public class InjuredInfo : MonoBehaviour
{
    public event Action<float, Vector3, Vector3> OnDamaged;


    public void OnBeingAttacked(AttackInfo attackInfo, Vector3 hitPoint, Vector3 hitNormal)
    {
        OnDamaged?.Invoke(attackInfo.DamagePoint, hitPoint, hitNormal);
    }

}

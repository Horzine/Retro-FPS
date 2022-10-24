using System;
using System.Collections.Generic;
using UnityEngine;

public class InjuredInfo : MonoBehaviour
{
    private readonly List<AttackerInfo> _cachedAttackInfo = new();
    public event Action<float> OnDamaged;

    void Update()
    {
        if (_cachedAttackInfo.Count > 0)
        {
            float combineDamage = 0;
            _cachedAttackInfo.ForEach(atk => combineDamage += atk.DamagePoint);
            OnDamaged?.Invoke(combineDamage);
            _cachedAttackInfo.Clear();
        }
    }

    public void OnBeingAttacked(AttackerInfo attackInfo)
    {
        _cachedAttackInfo.Add(attackInfo);
    }

}

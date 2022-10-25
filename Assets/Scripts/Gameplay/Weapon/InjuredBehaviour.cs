using System;
using UnityEngine;

public class InjuredInfo
{
    public AttackInfo AttackInfo { get; set; }
    public Vector3 HitPoint { get; set; }
    public Vector3 HitNormal { get; set; }
    public override string ToString()
    {
        return $"--InjuredInfo-- AttackInfo: {AttackInfo}, HitPoint: {HitPoint}, HitNormal: {HitNormal}";
    }

}
public class InjuredBehaviour : MonoBehaviour
{
    public event Action<InjuredInfo> OnDamaged;

    public void OnBeingAttacked(AttackInfo attackInfo, Vector3 hitPoint, Vector3 hitNormal)
    {
        OnDamaged?.Invoke(new InjuredInfo() { AttackInfo = attackInfo, HitPoint = hitPoint, HitNormal = hitNormal });
    }
}

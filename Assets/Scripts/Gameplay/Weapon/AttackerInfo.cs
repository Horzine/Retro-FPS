using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerInfo
{
    public bool MultiRayCast { get; set; }
    public float MaxDistance { get; set; }
    public float DamagePoint { get; internal set; }

    public static void SendAttackInfoToTarger(Component target, AttackerInfo attackInfo)
    {
        if (target.TryGetComponent<InjuredInfo>(out var injured))
        {
            injured.OnBeingAttacked(attackInfo);
        }
    }
}

using UnityEngine;

public class AttackInfo
{
    public bool MultiRayCast { get; set; }
    public float MaxDistance { get; set; }
    public int DamagePoint { get; set; }
    public Vector3 HitPoint { get; set; }
    public Vector3 HitNormal { get; set; }

    public static void SendAttackInfoToTarger(Component target, AttackInfo attackInfo, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (target.TryGetComponent<InjuredBehaviour>(out var injured))
        {
            injured.OnBeingAttacked(attackInfo, hitPoint, hitNormal);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig_", menuName = "ScriptableObjects/GunConfig", order = 1)]
public class GunConfig : ScriptableObject
{
    public enum BulletTypeEnum
    {
        RayScan = 0,
        Projectile = 1,
    }
    public enum FireModeEnum
    {
        FullAuto = 0,
        SemiAuto = 1,
    }

    public string GunName;
    public int DamagePoint;
    public FireModeEnum FireMode;
    public bool NeedBolt;
    public float BoltTime;
    public int MaxMagzineAmmo;
    public int MaxBackupAmmo;
    public int AmmoUsePerFire = 1;
    public BulletTypeEnum BulletType;
    public string BulletProjectileName;
    public float BulletMaxDistance;
    public float FireRoundsPerMinute = 1;
    public float ReloadSpeedMultiple = 1;
    public Sprite[] IdleAnim;
    public Sprite[] FireAnim;
    public Sprite[] ReloadAnim;
    public Sprite[] BoltAnim;

    public float FireIntervalTime => 60 / FireRoundsPerMinute;
    public bool HasSelfReloadAnim => ReloadAnim != null && ReloadAnim.Length > 0;
    public bool HasSelfBoltAnim => BoltAnim != null && BoltAnim.Length > 0;

}
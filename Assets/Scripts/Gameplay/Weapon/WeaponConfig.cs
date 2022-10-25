using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig_", menuName = "ScriptableObjects/WeaponConfig", order = 1)]
public class WeaponConfig : ScriptableObject
{
    public enum BulletTypeEnum
    {
        RayScan = 0,
        Projectile = 1,
    }
    public string WeaponName;
    public int DamagePoint;
    public int MaxBackupAmmo;
    public int MaxMagzineAmmo;
    public int AmmoUsePerFire = 1;
    public BulletTypeEnum BulletType;
    public string BulletProjectileName;
    public float BulletMaxDistance;
    public float FireRoundsPerMinute = 1;
    public float ReloadSpeedMultiple = 1;
    public Sprite[] IdleAnim;
    public Sprite[] FireAnim;
    public Sprite[] ReloadAnim;

    public float FireIntervalTime => 60 / FireRoundsPerMinute;
    public bool HasSelfReloadAnim => ReloadAnim != null && ReloadAnim.Length > 0;
  
}
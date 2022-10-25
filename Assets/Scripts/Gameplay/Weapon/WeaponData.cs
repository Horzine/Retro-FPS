using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData_", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public enum BulletTypeEnum
    {
        RayScan = 0,
        Projectile = 1,
    }
    public string WeaponName;
    public int DamanagePoint;
    public int MaxBackupAmmo;
    public int CurrentBackupAmmo;
    public int MaxMagzineAmmo;
    public int CurrentMagzineAmmo;
    public int AmmoUsePerFire = 1;
    public BulletTypeEnum BulletType;
    public string BulletProjectileName;
    public float FireRoundsPerMinute = 1;
    public float ReloadSpeedMultiple = 1;
    public Sprite[] IdleAnim;
    public Sprite[] FireAnim;
    public Sprite[] ReloadAnim;

    public float FireIntervalTime => 60 / FireRoundsPerMinute;
    public bool HasSelfReloadAnim => ReloadAnim != null && ReloadAnim.Length > 0;
    public bool CanReload => !(CurrentMagzineAmmo == MaxMagzineAmmo || CurrentBackupAmmo == 0);
    public void ReloadAmmunition()
    {
        if (CanReload)
        {
            var total = CurrentMagzineAmmo + CurrentBackupAmmo;
            if (total >= MaxMagzineAmmo)
            {
                CurrentMagzineAmmo = MaxMagzineAmmo;
                CurrentBackupAmmo = total - MaxMagzineAmmo;
            }
            else
            {
                CurrentMagzineAmmo = total;
                CurrentBackupAmmo = 0;
            }
        }
    }
    public bool CanFire => CurrentMagzineAmmo >= AmmoUsePerFire;
    public void Fire()
    {
        if (CanFire)
        {
            CurrentMagzineAmmo -= AmmoUsePerFire;
        }
    }
}
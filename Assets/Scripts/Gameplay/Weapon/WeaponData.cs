public class WeaponData
{
    public int CurrentMagzineAmmo { get; private set; }
    public int CurrentBackupAmmo { get; private set; }
    private WeaponConfig _config;
    public void BindConfig(WeaponConfig config)
    {
        _config = config;

        Init();
    }

    private void Init()
    {
        CurrentMagzineAmmo = _config.MaxMagzineAmmo;
        CurrentBackupAmmo = _config.MaxBackupAmmo;
    }

    public bool CanReload => !(CurrentMagzineAmmo == _config.MaxMagzineAmmo || CurrentBackupAmmo == 0);

    public void ReloadAmmunition()
    {
        if (CanReload)
        {
            var total = CurrentMagzineAmmo + CurrentBackupAmmo;
            if (total >= _config.MaxMagzineAmmo)
            {
                CurrentMagzineAmmo = _config.MaxMagzineAmmo;
                CurrentBackupAmmo = total - _config.MaxMagzineAmmo;
            }
            else
            {
                CurrentMagzineAmmo = total;
                CurrentBackupAmmo = 0;
            }
        }
    }

    public bool CanFire => CurrentMagzineAmmo >= _config.AmmoUsePerFire;

    public void Fire()
    {
        if (CanFire)
        {
            CurrentMagzineAmmo -= _config.AmmoUsePerFire;
        }
    }
}

using Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoSingleton<WeaponSystem>
{
    public enum WeaponEnum
    {
        Revolver = 0,
        AssaultRifle = 1,
        Shotgun = 2,

        Total_Weapon_Count,
    }

    private SortedList<int, IWeapon> _weaponDic;

    protected override void Awake()
    {
        base.Awake();

        _weaponDic = new(TotalWeaponEnumCount);
    }

    public int TotalWeaponEnumCount => (int)WeaponEnum.Total_Weapon_Count;

    public void InitWeaponList(List<WeaponEnum> weaponEnums, ref List<IWeapon> weaponList)
    {
        foreach (var item in weaponEnums)
        {
            var weapon = AddWeapon(item, ref weaponList);
        }
    }

    public IWeapon AddWeapon(WeaponEnum weaponEnum, ref List<IWeapon> weaponList)
    {
        if (!_weaponDic.TryGetValue((int)weaponEnum, out var weapon))
        {
            weapon = WeaponFactory.CreateWeapon(weaponEnum);
        }

        if (weapon != null)
        {
            weapon.WeaponEnum = weaponEnum;
            _weaponDic.Add((int)weaponEnum, weapon);

            if (weaponList != null)
            {
                weaponList.Add(weapon);
            }
        }
        return weapon;
    }

    public bool RemoveWeapon(WeaponEnum weaponEnum, ref List<IWeapon> weaponList)
    {
        if (_weaponDic.TryGetValue((int)weaponEnum, out var weapon))
        {
            _weaponDic.Remove((int)weapon.WeaponEnum);
            weaponList.Remove(weapon);
            return true;
        }
        else
        {
            return false;
        }
    }

    public IWeapon GetWeapon(WeaponEnum weaponEnum)
    {
        _weaponDic.TryGetValue((int)weaponEnum, out var weapon);
        return weapon;
    }

    private class WeaponFactory
    {
        public static IWeapon CreateWeapon(WeaponEnum weaponEnum)
        {
            var asset = AssetsManager.Instance.LoadComponent<WeaponBase>($"Assets/Resources/Prefab/Weapon/{weaponEnum}.prefab");

            if (asset && asset is IWeapon)
            {
                return Instantiate(asset).GetComponent<IWeapon>();
            }
            else
            {
                Debug.LogError($"WeaponSystem.WeaponFactory: CreateWeapon: {weaponEnum} asset no found");
                return null;
            }
        }
    }
}



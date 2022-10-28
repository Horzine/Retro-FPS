﻿using UnityEngine;

public interface IWeapon
{
    enum WeaponTypeEnum
    {
        Gun = 0,
        Melee = 1,
    }
    WeaponTypeEnum WeaponType { get; }
    WeaponSystem.WeaponEnum WeaponEnum { get; set; }
    bool IsSwaping { get; }
    void OnSwapOutEnd();
    void OnSwapIn();
    void OnSwapOut();
    void OnSwapInEnd();
    GameObject GameObject { get; }
}
public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    public abstract IWeapon.WeaponTypeEnum WeaponType { get; }
    public WeaponSystem.WeaponEnum WeaponEnum { get; set; }
    public bool IsSwaping { get; set; }
    public GameObject GameObject => gameObject;

    public virtual void OnSwapIn()
    {
        this.SetGameObjectActive(true);
        IsSwaping = true;
    }

    public virtual void OnSwapInEnd()
    {
        IsSwaping = false;
    }

    public virtual void OnSwapOut()
    {
        IsSwaping = true;
    }

    public virtual void OnSwapOutEnd()
    {
        this.SetGameObjectActive(false);
        IsSwaping = false;
    }
}
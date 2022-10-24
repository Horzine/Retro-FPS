using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AppliqueType
{
    Bullet_Hole = 0,
    Blood = 1,
}

public class AppliqueManager : MonoSingleton<AppliqueManager>
{
    private Applique _bulletHoleTemplete;

    protected override void Awake()
    {
        _bulletHoleTemplete = AssetsManager.Instance.LoadAssets<Applique>("Assets/Resources/Prefab/Applique/Applique_BulletHole.prefab");
        // AssetsManager.Instance.LoadAssetsAsync<Applique>("Assets/Resources/Prefab/Applique/Applique_BulletHole.prefab", (obj) => _bulletHoleTemplete = obj);
    }

    public void SpawnApplique(Transform targetTransform, Vector3 point, Vector3 normal)
    {
        var go = Instantiate(_bulletHoleTemplete, point + (normal * 0.01f), Quaternion.LookRotation(-normal) /*Random Rotation Here* Quaternion.Euler(0, 0, 39)*/);
        // Static Env show Applique
        // Movement item Show Sprite paritcal Effect
        // go.transform.SetParent(targetTransform, true);
    }
}

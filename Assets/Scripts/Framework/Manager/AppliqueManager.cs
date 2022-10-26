using UnityEngine;

namespace Framework
{
    enum AppliqueType
    {
        Quad = 0,
        Sprite = 1,
    }

    public class AppliqueManager : MonoSingleton<AppliqueManager>
    {
        private Applique _bulletHoleTemplete;
        private GameObjectPool<Applique> _appliquePool ;

        protected override void Awake()
        {
            _bulletHoleTemplete = AssetsManager.Instance.LoadComponent<Applique>("Assets/Resources/Prefab/Applique/Applique_BulletHole.prefab");
            // AssetsManager.Instance.LoadAssetsAsync<Applique>("Assets/Resources/Prefab/Applique/Applique_BulletHole.prefab", (obj) => _bulletHoleTemplete = obj);

            _appliquePool = new GameObjectPool<Applique>(_bulletHoleTemplete);
        }

        public void SpawnApplique(Transform targetTransform, Vector3 point, Vector3 normal)
        {
            // var go = Instantiate(_bulletHoleTemplete, point + (normal * 0.01f), Quaternion.LookRotation(-normal) /*Random Rotation Here* Quaternion.Euler(0, 0, 39)*/);
            // Static Env show Applique
            // Movement item Show Sprite paritcal Effect
            // go.transform.SetParent(targetTransform, true);

            var go = _appliquePool.GetObject();
            go.transform.SetPositionAndRotation(point + (normal * 0.01f), Quaternion.LookRotation(-normal) /*Random Rotation Here* Quaternion.Euler(0, 0, 39)*/);
        }

        private void OnDestroy()
        {
            _appliquePool.OnDestory();
        }
    }
}

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
        private GameObjectPool<Applique> _appliquePool;

        protected override void Awake()
        {
            _bulletHoleTemplete = AssetsManager.Instance.LoadComponent<Applique>("Assets/Resources/Prefab/Applique/Applique_BulletHole.prefab");
            _appliquePool = new GameObjectPool<Applique>(_bulletHoleTemplete, transform);
        }

        public void SpawnApplique(Vector3 point, Vector3 normal)
        {
            var go = _appliquePool.GetObject();
            go.transform.SetPositionAndRotation(point + (normal * 0.01f), Quaternion.LookRotation(-normal) /*Random Rotation Here* Quaternion.Euler(0, 0, 39)*/);
            // Movement item Show Sprite paritcal Effect
            // Static Env show Applique
        }

        private void OnDestroy()
        {
            _appliquePool.OnDestory();
        }
    }
}

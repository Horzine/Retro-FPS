using UnityEngine;

public class Test_Cube : MonoBehaviour
{
    private InjuredInfo _injuredInfo;

    private void Awake()
    {
        _injuredInfo = GetComponent<InjuredInfo>();
        _injuredInfo.OnDamaged += OnDamaged;
    }
    private void OnDestroy()
    {
        if (_injuredInfo)
        {
            _injuredInfo.OnDamaged -= OnDamaged;
        }
    }

    private void OnDamaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        this.PrintLog($"{nameof(OnDamaged)}", $"{nameof(damage)}: {damage}");
        AppliqueManager.Instance.SpawnApplique(transform, hitPoint, hitNormal);
    }
}

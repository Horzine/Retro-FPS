using UnityEngine;

public class Test_Cube : MonoBehaviour
{
    private InjuredBehaviour _injuredInfo;

    private void Awake()
    {
        _injuredInfo = GetComponent<InjuredBehaviour>();
        _injuredInfo.OnDamaged += OnDamaged;
    }
    private void OnDestroy()
    {
        if (_injuredInfo)
        {
            _injuredInfo.OnDamaged -= OnDamaged;
        }
    }

    private void OnDamaged(InjuredInfo injuredInfo)
    {
        // this.PrintLog($"{nameof(OnDamaged)}", $"{injuredInfo}");
        AppliqueManager.Instance.SpawnApplique(transform, injuredInfo.HitPoint, injuredInfo.HitNormal);
    }
}

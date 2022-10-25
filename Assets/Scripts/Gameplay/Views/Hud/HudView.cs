using UnityEngine;

public abstract class HudViewEntry : MonoBehaviour
{
    public abstract void OnInit();
    public abstract void OnUpdate();
}
public class HudView : MonoBehaviour
{
    [SerializeField] private HudViewEntry _weaponView;

    private void Start()
    {
        _weaponView.OnInit();
    }

    private void FixedUpdate()
    {
        _weaponView.OnUpdate();
    }
}

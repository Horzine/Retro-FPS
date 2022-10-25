using UnityEngine;

public class GameplayUICanvas : MonoBehaviour
{
    [SerializeField] private HudView _hudPanel;
    [SerializeField] private MenuView _menuPanel;

    private void Start()
    {
        _hudPanel.SetGameObjectActive(true);
        _menuPanel.SetGameObjectActive(false);
    }
}

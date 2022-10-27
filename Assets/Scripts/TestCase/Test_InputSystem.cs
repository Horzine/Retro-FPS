using Framework;
using UnityEngine;

public class Test_InputSystem : MonoBehaviour
{
    private void Start()
    {
        PlayerInputSystem.Instance.FireActionDown += () => print("FireAction");
        PlayerInputSystem.Instance.SecondaryFireAction += () => print("SecondaryFireAction");
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"MoveDirection: {PlayerInputSystem.Instance.MovementDirection}");
        GUI.Label(new Rect(10, 110, 300, 20), $"MouseAxisX: {PlayerInputSystem.Instance.MouseAxisX}");
        GUI.Label(new Rect(10, 210, 300, 20), $"MouseAxisY: {PlayerInputSystem.Instance.MouseAxisY}");
    }
}

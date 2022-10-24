using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_InputSystem : MonoBehaviour
{
    public PlayerInputSystem inputSystem;
    private void Start()
    {
        inputSystem.FireAction += () => print("FireAction");
        inputSystem.SecondaryFireAction += () => print("SecondaryFireAction");
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"MoveDirection: {inputSystem.MovementDirection}");
        GUI.Label(new Rect(10, 110, 300, 20), $"MouseAxisX: {inputSystem.MouseAxisX}");
        GUI.Label(new Rect(10, 210, 300, 20), $"MouseAxisY: {inputSystem.MouseAxisY}");
    }
}

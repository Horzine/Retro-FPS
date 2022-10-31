using Framework;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    private Animator _animator;
    private float _x;
    private float _y;
    private float _speed = 3;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void HandlePlayerInputMouseAxis(float mouseAxisX, float mouseAxisY)
    {
        _x = Mathf.MoveTowards(_x, mouseAxisX, _speed * Time.deltaTime);
        _y = Mathf.MoveTowards(_y, mouseAxisY, _speed * Time.deltaTime);

        _animator.SetFloat("AxisX", Mathf.Clamp(_x, -1, 1));
        _animator.SetFloat("AxisY", Mathf.Clamp(_y, -1, 1));
    }
}

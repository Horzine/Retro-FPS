using Framework;
using UnityEngine;

public class Test_Timer : MonoBehaviour
{
    public int _timer_1;
    [SerializeField] private float _inver_1;
    [SerializeField] private bool _loop_1;
    public bool ShouldTrigger;
    private void Start()
    {
        Debug.Log("Start Time");
        _timer_1 = TimerManager.Instance.Register(_inver_1, () => Callback(1), _loop_1);
    }

    private void Callback(int i)
    {
       //  Debug.Log($"Callback {i}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TimerManager.Instance.ResetTimerTime(_timer_1);
            Debug.Log("ResetTimerTime");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TimerManager.Instance.ForceTriggerTimer(_timer_1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TimerManager.Instance.CloseTimer(_timer_1, ShouldTrigger);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TimerManager.Instance.SetTimerInterval(_timer_1, _inver_1);
            TimerManager.Instance.SetTimerLoop(_timer_1, _loop_1);
        }
    }
}

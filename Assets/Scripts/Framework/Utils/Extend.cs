using UnityEngine;

public static class Extend
{
    public static void PrintLog(this MonoBehaviour monoBehaviour, string MethodName, string content)
    {
        Debug.Log($"{monoBehaviour.name}->{MethodName}-> {content}", monoBehaviour);
    }

    public static void PrintError(this MonoBehaviour monoBehaviour, string MethodName, string content)
    {
        Debug.LogError($"{monoBehaviour.name}->{MethodName}-> {content}", monoBehaviour);
    }

    public static void SetGameObjectActive(this Component component, bool active)
    {
        if (component && component.gameObject)
        {
            component.gameObject.SetActive(active);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public static class ExtensionMethod
{
    public static void Set(this CinemachineFreeLook cinemachineFreeLook, Transform transform)
    {
        if(cinemachineFreeLook != null)
        {
            cinemachineFreeLook.LookAt = transform;
            cinemachineFreeLook.Follow = transform;
        }
    }

    public static void Set(this Text text, string value)
    {
        if (text != null)
        {
            text.text = value;
        }
    }

    public static void Set(this Button button, string value)
    {
        if (button != null)
        {
            button.GetComponentInChildren<Text>().Set(value);
        }
    }
}
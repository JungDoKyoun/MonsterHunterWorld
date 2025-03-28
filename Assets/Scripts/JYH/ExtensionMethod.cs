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

    public static void SetText(this Text text, string value)
    {
        if (text != null)
        {
            text.text = value;
        }
    }

    public static void SetText(this Button button, string value)
    {
        if (button != null)
        {
            button.GetComponentInChildren<Text>().SetText(value);
        }
    }

    public static void SetInteractable(this Button button, bool value)
    {
        if (button != null)
        {
            button.interactable = value;
        }
    }

    public static void SetActive(this Button button, bool value)
    {
        if(button != null)
        {
            button.gameObject.SetActive(value);
        }
    }

    public static void SetActive(this Transform transform, bool value)
    {
        if(transform != null)
        {
            transform.gameObject.SetActive(value);
        }
    }

    public static void Set(this GameObject gameObject, bool value)
    {
        if(gameObject != null)
        {
            gameObject.SetActive(value);
        }
    }
}
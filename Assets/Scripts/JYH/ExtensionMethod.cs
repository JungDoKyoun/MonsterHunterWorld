using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections.Generic;

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

    public static void SetEnabled(this CinemachineFreeLook cinemachineFreeLook, bool value)
    {
        if(cinemachineFreeLook != null)
        {
            cinemachineFreeLook.enabled = value;
        }
    }

    public static void SetText(this Text text, string value)
    {
        if (text != null)
        {
            text.text = value;
        }
    }

    public static void SetColor(this Image image, Color color)
    {
        if(image != null)
        {
            image.color = color;
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

    public static void SetEnabled(this PlayerController playerController, bool value)
    {
        if(playerController != null)
        {
            playerController.enabled = value;
        }
    }

    public static void Sort<T>(ref T[] array) where T : Object
    {
        List<T> list = new List<T>();
        int empty = 0;
        int length = array != null ? array.Length : 0;
        for (int i = 0; i < length; i++)
        {
            T value = array[i];
            if (value != null)
            {
                if (list.Contains(value) == false)
                {
                    list.Add(value);
                }
                else
                {
                    empty++;
                }
            }
            else
            {
                empty++;
            }
        }
        for (int i = 0; i < empty; i++)
        {
            list.Add(null);
        }
        array = list.ToArray();
    }
}
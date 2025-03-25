using UnityEngine;
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
}
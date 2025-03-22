using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerCostume : MonoBehaviour
{
    private enum Index: byte
    {
        HeadStart = 0,
        HeadEnd = 1,
        BreastStart = 2,
        BreastEnd = 4,
        HandStart = 5,
        HandEnd = 7,
        Waist = 8,
        LegStart = 9,
        LegEnd = 10
    }

    [SerializeField]
    private SkinnedMeshRenderer[] _skinnedMeshRenderers = null;

#if UNITY_EDITOR

    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer = null;

    private void OnValidate()
    {
        Sort(ref _skinnedMeshRenderers);
    }

    private void Sort<T>(ref T[] array) where T: Component
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


    [ContextMenu("ShowLocalCenter")]
    public void ShowLocalCenter()
    {
        if (_skinnedMeshRenderer)
        {
            Debug.Log(_skinnedMeshRenderer.transform.InverseTransformPoint(_skinnedMeshRenderer.bounds.center));
        }
    }
#endif

}
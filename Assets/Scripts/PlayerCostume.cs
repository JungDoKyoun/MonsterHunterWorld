using UnityEngine;

[DisallowMultipleComponent]
public class PlayerCostume : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _helmRenderers = null;

#if UNITY_EDITOR
    private void OnValidate()
    {
        
    }
#endif

    public void Set()
    {

    }
}
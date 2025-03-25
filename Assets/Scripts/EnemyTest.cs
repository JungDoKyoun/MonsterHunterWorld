using UnityEngine;

[DisallowMultipleComponent]
public class EnemyTest : MonoBehaviour
{
    private bool _hasTransform = false;

    private Transform _transform = null;

    private Transform getTransform
    {
        get
        {
            if (_hasTransform == false)
            {
                _transform = transform;
                _hasTransform = true;
            }
            return _transform;
        }
    }

    [SerializeField]
    private bool _knockback = false;

    [SerializeField]
    private uint _damage = 1;

    [SerializeField]
    PlayerController _playerController = null;

#if UNITY_EDITOR
    [SerializeField]
    private Color _gizmoColor = Color.blue;

    private void OnDrawGizmos()
    {
        if (_playerController != null)
        {
            Vector3 position = getTransform.position;
            Vector3 direction = _playerController.transform.position - position;
            Gizmos.color = _gizmoColor;
            Gizmos.DrawRay(position, direction);
        }
    }
#endif

    [ContextMenu("АјАн")]
    public void Attack()
    {
        Vector3 position = getTransform.position;
        _playerController?.TakeDamage(position, _damage, _knockback);
    }
}
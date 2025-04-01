using UnityEngine;

[DisallowMultipleComponent]
public class InteractionTest : MonoBehaviour
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
    private int _damage = 1;

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

    [ContextMenu("공격")]
    public void Attack()
    {
        Vector3 position = getTransform.position;
        _playerController?.TakeDamage(position, _damage, _knockback);
    }

    [ContextMenu("체력 회복")]
    public void RecoverLife()
    {
        _playerController?.TryRecover(10, 2);
    }

    [ContextMenu("스태미나 회복")]
    public void RecoverStamina()
    {
        _playerController?.TryRecover(10f);
    }
#endif
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds = 2f;

    void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
}

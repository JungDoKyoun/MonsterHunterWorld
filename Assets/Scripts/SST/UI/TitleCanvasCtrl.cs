using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCanvasCtrl : MonoBehaviour
{
    [SerializeField] GameObject loginCanvas;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            loginCanvas.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}

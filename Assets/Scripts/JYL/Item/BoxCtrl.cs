using UnityEngine;


public class BoxCtrl : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //activeButton.SetActive(true);
            //UIManager.Instance.OpenUI(UIType.BoxSelectUI);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIManager.Instance.isBox = true;
            UIManager.Instance.player = other;

        }

        //Debug.Log(isPlayer);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIManager.Instance.isBox = false;          
            //UIManager.Instance.CloseUI(UIType.BoxSelectUI);

        }
    }

}

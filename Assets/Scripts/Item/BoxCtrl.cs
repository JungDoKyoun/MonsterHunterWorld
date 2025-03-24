using UnityEngine;

public class BoxCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject invenCanvas;//�κ�ĵ����

    [SerializeField]
    GameObject activeButton;//Ȱ��ȭ ��ư

    [SerializeField]
    BoxInvenTory boxInvenTory;//�繰�� �κ��丮

    // Start is called before the first frame update
    void Start()
    {
        invenCanvas.gameObject.SetActive(false);
        activeButton.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            activeButton.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (invenCanvas.gameObject.activeSelf == false)
                {
                    invenCanvas.gameObject.SetActive(true);
                    boxInvenTory.IsBoxOpen = true;
                    activeButton.SetActive(false);
                    other.gameObject.GetComponent<PlayerController>().enabled = false;

                }
                else
                {
                    invenCanvas.gameObject.SetActive(false);
                    boxInvenTory.IsBoxOpen = false;
                    other.gameObject.GetComponent<PlayerController>().enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            activeButton.SetActive(false);
        }
    }

}

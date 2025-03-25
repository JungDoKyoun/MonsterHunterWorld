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
                Debug.Log("f");
                if (invenCanvas.gameObject.activeSelf == false)
                {
                    invenCanvas.gameObject.SetActive(true);
                    Debug.Log("�� ��������?");
                    activeButton.SetActive(false);

                    //�÷��̾� �̵� ����
                    other.gameObject.GetComponent<PlayerController>().enabled = false;
                    other.gameObject.GetComponent<PlayerController>().Move(Vector2.zero);

                    boxInvenTory.OpenBox();
                }
                else
                {
                    invenCanvas.gameObject.SetActive(false);
                    other.gameObject.GetComponent<PlayerController>().enabled = true;
                    boxInvenTory.InvenClose();
                    activeButton.SetActive(true);

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

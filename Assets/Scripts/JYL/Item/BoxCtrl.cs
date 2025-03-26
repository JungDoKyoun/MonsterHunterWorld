using UnityEngine;


public class BoxCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject invenCanvas;//인벤캔버스

    [SerializeField]
    GameObject activeButton;//활성화 버튼

    [SerializeField]
    BoxInvenTory boxInvenTory;//사물함 인벤토리

    bool isPlayer = false;

    Collider player;

    void Start()
    {
        invenCanvas.gameObject.SetActive(false);
        activeButton.SetActive(false);
    }

    private void Update()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F");

            if (invenCanvas.gameObject.activeSelf == false)
            {
                invenCanvas.gameObject.SetActive(true);
                //Debug.Log("왜 안켜지지?");
                activeButton.SetActive(false);

                //플레이어 이동 제한
                player.gameObject.GetComponent<PlayerController>().enabled = false;
                player.gameObject.GetComponent<PlayerController>().Move(Vector2.zero);

                boxInvenTory.OpenBox();
            }
            else
            {
                invenCanvas.gameObject.SetActive(false);
                player.gameObject.GetComponent<PlayerController>().enabled = true;
                boxInvenTory.InvenClose();
                activeButton.SetActive(true);

            }
        }
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
            isPlayer = true;
            player = other;

        }

        Debug.Log(isPlayer);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            activeButton.SetActive(false);
            isPlayer = false;

        }
    }

}

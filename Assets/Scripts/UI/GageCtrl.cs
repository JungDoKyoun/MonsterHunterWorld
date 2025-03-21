using UnityEngine;

public class GageCtrl : MonoBehaviour
{
    [SerializeField] FillBar hpBar;

    [SerializeField] FillBar spBar;

    PlayerController playerCtrl;

    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GetComponent<PlayerController>();
        hpBar.SetPlayer(playerCtrl);
        spBar.SetPlayer(playerCtrl);
    }

    public void UpdateHP(float newHP)
    {
        hpBar.UpdateHP(newHP);
    }


}

using UnityEngine;

public class GageCtrl : MonoBehaviour
{
    FillBar hpBar;

    FillBar spBar;

    PlayerController playerCtrl;

    // Start is called before the first frame update
    void Start()
    {        
        playerCtrl = GetComponent<PlayerController>();
        hpBar = GameObject.Find("HpBar").GetComponent<FillBar>();
        spBar = GameObject.Find("StaminaBar ").GetComponent<FillBar>();
        hpBar.SetPlayer(playerCtrl);
        spBar.SetPlayer(playerCtrl);
        GameObject.Find("HUDCanvas").SetActive(false);
    }

    public void UpdateHP(float newHP)
    {
        hpBar.UpdateHP(newHP);
    }


}

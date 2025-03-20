using UnityEngine;

public enum InvenType
{
    Inven,
    Box
}

//플레이어의 모든 인벤토리
public class InvenToryCtrl : MonoBehaviour
{
    //현재 가지고있을 인벤토리
    public InventoryItems inventoryItems;
    //사물함 인벤토리
    public BoxInvenTory boxInvenTory;

    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ChangeItem(InvenType type, BaseItem item)
    {
        //인벤토리에서 아이템을 선택했을때
        if (type == InvenType.Inven)
        {
            inventoryItems.Items.Remove(item);
            boxInvenTory.BoxItems.Add(item);

        }
        //사물함에서 아이템을 선택했을때
        else
        {
            //아이템이 있으면
            //아이템을 인벤토리로 이동
            boxInvenTory.BoxItems.Remove(item);
            inventoryItems.Items.Add(item);
        }

    }

}

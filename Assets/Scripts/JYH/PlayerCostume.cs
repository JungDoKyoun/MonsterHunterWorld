using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

[DisallowMultipleComponent]
public class PlayerCostume : MonoBehaviour
{
    private enum Index: byte
    {
        Hand1Start = 0,
        Hand1End = 3,
        Hand2Start = 4,
        Hand2End = 8,
        Hand3Start = 9,
        Hand3End = 12,
        Hand4Start = 13,
        Hand4End = 15,
        Hand5Start = 16,
        Hand5End = 18,
        Hand6Start = 19,
        Hand6End = 21,
        Breast1Start = 22,
        Breast1End = 23,
        Breast2Start = 24,
        Breast2End = 26,
        Breast3Start = 27,
        Breast3End = 29,
        Breast4Start = 30,
        Breast4End = 31,
        Breast5Start = 32,
        Breast5End = 34,
        Breast6Start = 35,
        Breast6End = 37,
        Head1Start = 38,
        Head1End = 38,
        Head2Start = 39,
        Head2End = 40,
        Head3Start = 41,
        Head3End = 42,
        Head4Start = 43,
        Head4End = 44,
        Head5Start = 45,
        Head5End = 46,
        Head6Start = 47,
        Head6End = 48,
        Leg1Start = 49,
        Leg1End = 51,
        Leg2Start = 52,
        Leg2End = 54,
        Leg3Start = 55,
        Leg3End = 57,
        Leg4Start = 58,
        Leg4End = 60,
        Leg5Start = 61,
        Leg5End = 63,
        Leg6Start = 64,
        Leg6End = 65,
        Waist1Start = 66,
        Waist1End = 67,
        Waist2Start = 68,
        Waist2End = 69,
        Waist3Start = 70,
        Waist3End = 71,
        Waist4Start = 72,
        Waist4End = 73,
        Waist5Start = 74,
        Waist5End = 76,
        Waist6Start = 77,
        Waist6End = 77,
        OneHandSwordStart = 78,
        OneHandSwordEnd = 79,
    }

    public enum Weapon: byte
    {
        OneHandSword,
        GreatSword,
        End
    }

    [SerializeField]
    private Weapon _weapon = Weapon.OneHandSword;

    [SerializeField]
    private GameObject[] _costumes = new GameObject[0];

    private static readonly int SetIndexStart = 0;
    private static readonly int SetIndexEnd = 5;

    public const string WeaponTag = "Weapon";
    public const string HeadTag = "Head";
    public const string BreastTag = "Breast";
    public const string HandTag = "Hand";
    public const string WaistTag = "Waist";
    public const string LegTag = "Leg";

#if UNITY_EDITOR

    private void OnValidate()
    {
        ExtensionMethod.Sort(ref _costumes);
    }

    [ContextMenu("장비 없음")]
    private void Set0()
    {
        SetHand(-1);
        SetBreast(-1);
        SetHead(-1);
        SetLeg(-1);
        SetWaist(-1);
    }

    [ContextMenu("장비 1")]
    private void Set1()
    {
        SetHand(0);
        SetBreast(0);
        SetHead(0);
        SetLeg(0);
        SetWaist(0);
    }

    [ContextMenu("장비 2")]
    private void Set2()
    {
        SetHand(1);
        SetBreast(1);
        SetHead(1);
        SetLeg(1);
        SetWaist(1);
    }

    [ContextMenu("장비 3")]
    private void Set3()
    {
        SetHand(2);
        SetBreast(2);
        SetHead(2);
        SetLeg(2);
        SetWaist(2);
    }

    [ContextMenu("장비 4")]
    private void Set4()
    {
        SetHand(3);
        SetBreast(3);
        SetHead(3);
        SetLeg(3);
        SetWaist(3);
    }

    [ContextMenu("장비 5")]
    private void Set5()
    {
        SetHand(4);
        SetBreast(4);
        SetHead(4);
        SetLeg(4);
        SetWaist(4);
    }

    [ContextMenu("장비 6")]
    private void Set6()
    {
        SetHand(5);
        SetBreast(5);
        SetHead(5);
        SetLeg(5);
        SetWaist(5);
    }
#endif

    private void SetHand(int index, bool value)
    {
        switch(index)
        {
            case 0:
                for (int i = (int)Index.Hand1Start; i <= (int)Index.Hand1End; i++)
                {
                    if(i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 1:
                for (int i = (int)Index.Hand2Start; i <= (int)Index.Hand2End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 2:
                for (int i = (int)Index.Hand3Start; i <= (int)Index.Hand3End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 3:
                for (int i = (int)Index.Hand4Start; i <= (int)Index.Hand4End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 4:
                for (int i = (int)Index.Hand5Start; i <= (int)Index.Hand5End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 5:
                for (int i = (int)Index.Hand6Start; i <= (int)Index.Hand6End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
        }
    }

    private void SetBreast(int index, bool value)
    {
        switch (index)
        {
            case 0:
                for (int i = (int)Index.Breast1Start; i <= (int)Index.Breast1End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 1:
                for (int i = (int)Index.Breast2Start; i <= (int)Index.Breast2End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 2:
                for (int i = (int)Index.Breast3Start; i <= (int)Index.Breast3End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 3:
                for (int i = (int)Index.Breast4Start; i <= (int)Index.Breast4End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 4:
                for (int i = (int)Index.Breast5Start; i <= (int)Index.Breast5End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 5:
                for (int i = (int)Index.Breast6Start; i <= (int)Index.Breast6End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
        }
    }

    private void SetHead(int index, bool value)
    {
        switch (index)
        {
            case 0:
                for (int i = (int)Index.Head1Start; i <= (int)Index.Head1End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 1:
                for (int i = (int)Index.Head2Start; i <= (int)Index.Head2End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 2:
                for (int i = (int)Index.Head3Start; i <= (int)Index.Head3End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 3:
                for (int i = (int)Index.Head4Start; i <= (int)Index.Head4End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 4:
                for (int i = (int)Index.Head5Start; i <= (int)Index.Head5End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 5:
                for (int i = (int)Index.Head6Start; i <= (int)Index.Head6End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
        }
    }

    private void SetLeg(int index, bool value)
    {
        switch (index)
        {
            case 0:
                for (int i = (int)Index.Leg1Start; i <= (int)Index.Leg1End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 1:
                for (int i = (int)Index.Leg2Start; i <= (int)Index.Leg2End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 2:
                for (int i = (int)Index.Leg3Start; i <= (int)Index.Leg3End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 3:
                for (int i = (int)Index.Leg4Start; i <= (int)Index.Leg4End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 4:
                for (int i = (int)Index.Leg5Start; i <= (int)Index.Leg5End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 5:
                for (int i = (int)Index.Leg6Start; i <= (int)Index.Leg6End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
        }
    }

    private void SetWaist(int index, bool value)
    {
        switch (index)
        {
            case 0:
                for (int i = (int)Index.Waist1Start; i <= (int)Index.Waist1End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 1:
                for (int i = (int)Index.Waist2Start; i <= (int)Index.Waist2End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 2:
                for (int i = (int)Index.Waist3Start; i <= (int)Index.Waist3End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 3:
                for (int i = (int)Index.Waist4Start; i <= (int)Index.Waist4End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 4:
                for (int i = (int)Index.Waist5Start; i <= (int)Index.Waist5End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
            case 5:
                for (int i = (int)Index.Waist6Start; i <= (int)Index.Waist6End; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
        }
    }

    public void SetHand(int index)
    {
        for (int i = SetIndexStart; i <= SetIndexEnd; i++)
        {
            SetHand(i, index == i);
        }
    }

    public void SetBreast(int index)
    {
        for (int i = SetIndexStart; i <= SetIndexEnd; i++)
        {
            SetBreast(i, index == i);
        }
    }

    public void SetHead(int index)
    {
        for (int i = SetIndexStart; i <= SetIndexEnd; i++)
        {
            SetHead(i, index == i);
        }
    }

    public void SetLeg(int index)
    {
        for (int i = SetIndexStart; i <= SetIndexEnd; i++)
        {
            SetLeg(i, index == i);
        }
    }

    public void SetWaist(int index)
    {
        for (int i = SetIndexStart; i <= SetIndexEnd; i++)
        {
            SetWaist(i, index == i);
        }
    }

    public void SetWeapon(bool value)
    {
        switch (_weapon)
        {
            case Weapon.OneHandSword:
                for (int i = (int)Index.OneHandSwordStart; i <= (int)Index.OneHandSwordEnd; i++)
                {
                    if (i < _costumes.Length && _costumes[i] != null)
                    {
                        _costumes[i].SetActive(value);
                    }
                }
                break;
        }
    }

    public void Equip(ItemName itemKey)
    {
        switch(itemKey)
        {
            case ItemName.HuntersKnife_I:
                SetWeapon(true);
                break;
            case ItemName.LeatherHead:
                SetHead(0);
                break;
            case ItemName.LeatherVest:
                SetBreast(0);
                break;
            case ItemName.LeatherGloves:
                SetHand(0);
                break;
            case ItemName.LeatherBelt:
                SetWaist(0);
                break;
            case ItemName.LeatherPants:
                SetLeg(0);
                break;
            case ItemName.HuntersHelmS:
                SetHead(1);
                break;
            case ItemName.HuntersMailS:
                SetBreast(1);
                break;
            case ItemName.HuntersArmS:
                SetHand(1);
                break;
            case ItemName.HuntersCoilS:
                SetWaist(1);
                break;
            case ItemName.HuntersGreavesS:
                SetLeg(1);
                break;
            case ItemName.AlloyHelmS:
                SetHead(2);
                break;
            case ItemName.AlloyMailS:
                SetBreast(2);
                break;
            case ItemName.AlloyArmS:
                SetHand(2);
                break;
            case ItemName.AlloyCoilS:
                SetWaist(2);
                break;
            case ItemName.AlloyGreavesS:
                SetLeg(2);
                break;
            case ItemName.BoneHelmS:
                SetHead(3);
                break;
            case ItemName.BoneMailS:
                SetBreast(3);
                break;
            case ItemName.BoneArmS:
                SetHand(3);
                break;
            case ItemName.BoneCoilS:
                SetWaist(3);
                break;
            case ItemName.BoneGreavesS:
                SetLeg(3);
                break;
            case ItemName.KuluHelmS:
                SetHead(4);
                break;
            case ItemName.KuluMailS:
                SetBreast(4);
                break;
            case ItemName.KuluBraceS:
                SetHand(4);
                break;
            case ItemName.KuluCoilS:
                SetWaist(4);
                break;
            case ItemName.KuluGreavesS:
                SetLeg(4);
                break;
            case ItemName.AnjaHelmS:
                SetHead(5);
                break;
            case ItemName.AnjaMailS:
                SetBreast(5);
                break;
            case ItemName.AnjaArmS:
                SetHand(5);
                break;
            case ItemName.AnjaCoilS:
                SetWaist(5);
                break;
            case ItemName.AnjaGreavesS:
                SetLeg(5);
                break;
        }
    }

    public static List<int> GetEquipList(Hashtable hashtable)
    {
        List<int> list = new List<int>();
        if(hashtable != null)
        {
            foreach(string key in hashtable.Keys)
            {
                switch(key)
                {
                    case WeaponTag:
                        if (hashtable[key] != null && int.TryParse(hashtable[key].ToString(), out int weapon))
                        {
                            list.Add(weapon);
                        }
                        break;
                    case HandTag:
                        if (hashtable[key] != null && int.TryParse(hashtable[key].ToString(), out int hand))
                        {
                            list.Add(hand);
                        }
                        break;
                    case BreastTag:
                        if (hashtable[key] != null && int.TryParse(hashtable[key].ToString(), out int breast))
                        {
                            list.Add(breast);
                        }
                        break;
                    case HeadTag:
                        if (hashtable[key] != null && int.TryParse(hashtable[key].ToString(), out int head))
                        {
                            list.Add(head);
                        }
                        break;
                    case LegTag:
                        if (hashtable[key] != null && int.TryParse(hashtable[key].ToString(), out int leg))
                        {
                            list.Add(leg);
                        }
                        break;
                    case WaistTag:
                        if (hashtable[key] != null && int.TryParse(hashtable[key].ToString(), out int waist))
                        {
                            list.Add(waist);
                        }
                        break;
                }
            }
        }
        return list;
    }
}
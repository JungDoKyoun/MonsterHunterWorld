using System.Collections.Generic;
using UnityEngine;

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

    public static readonly string EquipmentTag = "Equipment";

#if UNITY_EDITOR

    private void OnValidate()
    {
        ExtensionMethod.Sort(ref _costumes);
    }

    [ContextMenu("장비 없음")]
    private void Set0()
    {
        Equip(-1);
    }

    [ContextMenu("장비 1")]
    private void Set1()
    {
        Equip(0);
    }

    [ContextMenu("장비 2")]
    private void Set2()
    {
        Equip(1);
    }

    [ContextMenu("장비 3")]
    private void Set3()
    {
        Equip(2);
    }

    [ContextMenu("장비 4")]
    private void Set4()
    {
        Equip(3);
    }

    [ContextMenu("장비 5")]
    private void Set5()
    {
        Equip(4);
    }

    [ContextMenu("장비 6")]
    private void Set6()
    {
        Equip(5);
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

    public void Equip(int index)
    {
        SetHand(index);
        SetBreast(index);
        SetHead(index);
        SetLeg(index);
        SetWaist(index);
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
}
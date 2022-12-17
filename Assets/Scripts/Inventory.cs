using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance = null;

    public int AtkBoostMultiplier = 2;
    public float AtkBoostTime = 5;
    public float ShieldTime = 10;
    public int HealAmount = 25;

    private int _atkBoost = 1;
    private int _shields = 1;
    private int _heals = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PickupItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.ATK_BOOST:
                _atkBoost++;
                break;

            case ItemType.SHIELD:
                _shields++;
                break;

            case ItemType.HEAL:
                _heals++;
                break;

            default:
                break;
        }
    }

    public bool CanUse(ItemType type)
    {
        switch(type)
        {
            case ItemType.ATK_BOOST:
                if (_atkBoost > 0)
                {
                    _atkBoost--;
                    return true;
                }
                break;

            case ItemType.SHIELD:
                if (_shields > 0)
                {
                    _shields--;
                    return true;
                }
                break;

            case ItemType.HEAL:
                if (_heals > 0)
                {
                    _heals--;
                    return true;
                }
                break;

            default:    
                return false;
        }

        return false;
    }
}

public enum ItemType
{
    ATK_BOOST,
    SHIELD,
    HEAL
}

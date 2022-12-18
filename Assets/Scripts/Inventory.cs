using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance = null;

    public bool HasReachedCheckpoint = false;
    public int AtkBoostMultiplier = 2;
    public float AtkBoostTime = 5;
    public float ShieldTime = 10;
    public int HealAmount = 25;

    private int _atkBoost = 0;
    private int _shields = 0;
    private int _heals = 0;
    public int AtkBoost => _atkBoost;
    public int Shields => _shields;
    public int Heals => _heals;
    private int[] _inventorySaveState = { 0, 0, 0 };  //Atk, shields, heals.

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

    public string PickupItem(ItemType type)
    {
        string itemName = "";
        switch (type)
        {
            case ItemType.ATK_BOOST:
                _atkBoost++;
                itemName = "Attack Boost";
                break;

            case ItemType.SHIELD:
                _shields++;
                itemName = "Shield";
                break;

            case ItemType.HEAL:
                _heals++;
                itemName = "Heal";
                break;

            default:
                break;
        }

        _inventorySaveState = new int[] { _atkBoost,  _shields, _heals};
        return $"{itemName} has been added to inventory!";
    }

    public bool CanUse(ItemType type)
    {
        bool canUseIt = false;
        switch(type)
        {
            case ItemType.ATK_BOOST:
                if (_atkBoost > 0)
                {
                    _atkBoost--;
                    canUseIt = true;
                }
                break;

            case ItemType.SHIELD:
                if (_shields > 0)
                {
                    _shields--;
                    canUseIt = true;
                }
                break;

            case ItemType.HEAL:
                if (_heals > 0)
                {
                    _heals--;
                    canUseIt = true;
                }
                break;

            default:
                break;
        }

        if (canUseIt)
        {
            GameLogicManager.Instance.UI.UseItem(type);
        }

        return canUseIt;
    }

    public void RestoreInventory()
    {
        _atkBoost = _inventorySaveState[0];
        _shields = _inventorySaveState[1];
        _heals = _inventorySaveState[2];
    }
}

public enum ItemType
{
    ATK_BOOST,
    SHIELD,
    HEAL
}

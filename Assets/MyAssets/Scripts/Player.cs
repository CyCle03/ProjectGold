using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject shop;
    public InventoryObject sell;
    public BuildingListObject buildList;
    public Slider HPSlider;
    public GameObject shopObj;
    public GameObject invenGoldObj;
    public GameObject sellGoldObj;

    public Attribute[] attributes;

    public float maxHP = 100;
    public float curruntHP;
    public float attackDamage;
    public float regenHP = 0;
    public int level = 1;
    public int exp = 0;
    public int s_agi;
    public int l_agi;
    public int s_int;
    public int l_int;
    public int s_stm;
    public int l_stm;
    public int s_str;
    public int l_str;

    GameManager gm;
    TextMeshProUGUI invenTextGold;
    TextMeshProUGUI sellTextGold;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
            switch (attributes[i].type)
            {
                case Attributes.Agility:
                    s_agi = attributes[i].value.ModifiedValue;
                    l_agi = i;
                    break;
                case Attributes.Intellect:
                    s_int = attributes[i].value.ModifiedValue;
                    l_int = i;
                    break;
                case Attributes.Stamina:
                    s_stm = attributes[i].value.ModifiedValue;
                    l_stm = i;
                    break;
                case Attributes.Strength:
                    s_str = attributes[i].value.ModifiedValue;
                    l_str = i;
                    break;
                default:
                    break;
            }
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
        for (int i = 0; i < sell.GetSlots.Length; i++)
        {
            sell.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            sell.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
        for (int i = 0; i < buildList.GetListSlots.Length; i++)
        {
            buildList.GetListSlots[i].OnBeforeUpdate += OnBeforeListSlotUpdate;
            buildList.GetListSlots[i].OnAfterUpdate += OnAfterListSlotUpdate;
        }
        UpdatePStats();
        curruntHP = maxHP;
        UpdateHPSlider();

        invenTextGold = invenGoldObj.GetComponent<TextMeshProUGUI>();
        sellTextGold = sellGoldObj.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (curruntHP <= maxHP)
        {
            curruntHP += (regenHP * Time.deltaTime / 10);
            UpdateHPSlider();
        }
    }

    public void InvenGoldUpdate()
    {
        invenTextGold.text = inventory.gold.ToString("n0") + " G";
    }

    public void SellGoldUpdate()
    {
        sellTextGold.text = sell.gold.ToString("n0") + " G";
    }

    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
        {
            return;
        }
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:

                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                        }
                    }
                }
                UpdatePStats();
                UpdateDmg();
                break;
            case InterfaceType.Chest:
                break;
            case InterfaceType.Shop:
                break;
            case InterfaceType.Sell:
                sell.RemoveGold(_slot.totalValue);
                SellGoldUpdate();
                break;
            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
        {
            return;
        }
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:

                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                        }
                    }
                }
                UpdatePStats();
                UpdateDmg();
                break;
            case InterfaceType.Chest:
                break;
            case InterfaceType.Shop:
                break;
            case InterfaceType.Sell:
                sell.AddGold(_slot.totalValue);
                SellGoldUpdate();
                break;
            default:
                break;
        }
    }

    public void OnBeforeListSlotUpdate(ListSlot _listSlot)
    {
        if (_listSlot.BuildObject == null)
        {
            return;
        }
        switch (_listSlot.parent.buildList.type)
        {
            case BuildInterfaceList.ETC:

                break;
            case BuildInterfaceList.BuildList:
                print(string.Concat("Removed ", _listSlot.BuildObject, " on ", _listSlot.parent.buildList.type, ", Allowed Building: ", string.Join(", ", _listSlot.AllowedBuilds)));

                for (int i = 0; i < _listSlot.build.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _listSlot.build.buffs[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_listSlot.build.buffs[i]);
                        }
                    }
                }
                break;
            case BuildInterfaceList.BuildShop:
                //buildShop.RemoveGold(_listSlot.build.BuildValue);
                //ShopGoldUpdate();
                break;
            default:
                break;
        }
    }

    public void OnAfterListSlotUpdate(ListSlot _listSlot)
    {
        if (_listSlot.BuildObject == null)
        {
            return;
        }
        switch (_listSlot.parent.buildList.type)
        {
            case BuildInterfaceList.ETC:

                break;
            case BuildInterfaceList.BuildList:
                print(string.Concat("Placed ", _listSlot.BuildObject, " on ", _listSlot.parent.buildList.type, ", Allowed Builds: ", string.Join(", ", _listSlot.AllowedBuilds)));

                for (int i = 0; i < _listSlot.build.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _listSlot.build.buffs[i].attribute)
                        {
                            attributes[j].value.AddModifier(_listSlot.build.buffs[i]);
                        }
                    }
                }
                UpdatePStats();
                UpdateDmg();
                break;
            case BuildInterfaceList.BuildShop:
                //buildShop.AddGold(_listSlot.build.BuildValue);
                //ShopGoldUpdate();
                break;
            default:
                break;
        }
    }

    public void ShopReset()
    {
        if (!ShopResetCheck())
        {
            print("Not enough slots on Inventory");
            return;
        }
        ShopAddInventory();
    }

    public bool ShopResetCheck()
    {
        if (sell.OnSlotCount > inventory.EmptySlotCount)
        {
            print("Not enough slots on Inventory");
            return false;
        }
        return true;
    }

    public void ShopAddInventory()
    {
        for (int i = 0; i < sell.GetSlots.Length; i++)
        {
            if (sell.GetSlots[i].item.Id >= 0)
            {
                Item _item = sell.GetSlots[i].item;
                int _amount = sell.GetSlots[i].amount;
                if (inventory.AddItem(_item, _amount))
                {
                    sell.GetSlots[i].RemoveItem();
                }
            }
        }
    }

    public void ShopSell()
    {
        inventory.AddGold(sell.gold);
        InvenGoldUpdate();
        sell.Clear();
    }

    public void ShopBuy(int _itemValue, Item _item, int _amount)
    {
        inventory.RemoveGold(_itemValue);
        inventory.AddItem(_item, _amount);
        InvenGoldUpdate();
    }

    public void EatFood(ItemObject _itemObj, int _amount)
    {
        int _foodHP = _itemObj.foodHP;
        print(_foodHP); 
        inventory.UseItem(_itemObj.data);
        if (maxHP - curruntHP >= _foodHP)
        { curruntHP += _foodHP; }   
        else
        { curruntHP = maxHP; }
        UpdateHPSlider();
    }

    public void UpdateDmg()
    {
        attackDamage = (float)(s_str + (s_agi * 0.5));
        print(string.Concat("Damage: ", attackDamage));
    }

    public float HitDamage()
    {
        UpdateDmg();
        print(attackDamage);
        return attackDamage;
    }

    public void UpdatePStats()
    {
        s_agi = GetStats(l_agi);
        s_int = GetStats(l_int);
        s_stm = GetStats(l_stm);
        s_str = GetStats(l_str);
        maxHP = 100+(s_stm * 10);
        regenHP = s_stm + (s_int * 0.5f);
        UpdateHPSlider();
    }

    public int GetStats(int l_stat)
    {
        int _stat = -1;
        if (attributes[l_stat] != null)
        {
            _stat = attributes[l_stat].value.ModifiedValue;
        }
        return _stat;
    }

    public void UpdateEXP(int GetExp)
    {
        exp += GetExp;
        if (exp <= (level * 10))
        {
            exp -= level * 10;
            level++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void GetDamaged(float damage)
    {
        curruntHP -= damage;
        UpdateHPSlider();
    }

    void UpdateHPSlider()
    {
        HPSlider.value = (float)curruntHP / (float)maxHP;
        gm.HPTextUpdate(curruntHP, maxHP);
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}
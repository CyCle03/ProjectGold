using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public enum Stat
{
    HP,
    Heal,
    Attack,
    AttackSpeed,
    MoveSpeed,
    MoneyEarn,
    Armor
}


public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject shop;
    public InventoryObject sell;
    public BuildingListObject buildList;
    public BuildingListObject shopList;
    public Slider HPSlider;
    public TextMeshProUGUI lvText;
    public TextMeshProUGUI expText;
    public Slider expSlider;
    public TextMeshProUGUI attText;
    public TextMeshProUGUI statText;
    public GameObject shopObj;
    public GameObject invenGoldObj;
    public GameObject sellGoldObj;

    public Attribute[] attributes;
    public PlayerStat[] stats;

    public float maxHP = 100;
    public float curruntHP;
    public float attackDamage;
    public float regenHP = 0;
    public int level = 1;
    public int exp = 0;

    public Dictionary<Stat, int> StatOnIndex = new Dictionary<Stat, int>();

    GameManager gm;
    BuildManager bm;
    TextMeshProUGUI invenTextGold;
    TextMeshProUGUI sellTextGold;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        bm = GameObject.Find("BuildManager").GetComponent<BuildManager>();

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].SetParent(this);
            //StatOnIndex
            switch (stats[i].type)
            {
                case Stat.HP:
                    stats[i].value.BaseValue = 100;
                    break;
                case Stat.Heal:
                    stats[i].value.BaseValue = 1;
                    break;
                case Stat.Attack:
                    stats[i].value.BaseValue = 1;
                    break;
                case Stat.AttackSpeed:
                    break;
                case Stat.MoveSpeed:
                    break;
                case Stat.MoneyEarn:
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
        for (int i = 0; i < shopList.GetListSlots.Length; i++)
        {
            shopList.GetListSlots[i].OnBeforeUpdate += OnBeforeListSlotUpdate;
            shopList.GetListSlots[i].OnAfterUpdate += OnAfterListSlotUpdate;
        }
        UpdatePStats();
        curruntHP = maxHP;
        UpdateHPSlider();
        UpdateEXP(0);

        invenTextGold = invenGoldObj.GetComponent<TextMeshProUGUI>();
        sellTextGold = sellGoldObj.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (curruntHP < maxHP)
        {
            curruntHP += (regenHP * Time.deltaTime / 10);
            UpdateHPSlider();
        }
    }

    public ModifiableInt GetAttributes(Attributes _attribute)
    {
        ModifiableInt _value = new ModifiableInt();
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i].type == _attribute)
            {
                _value = attributes[i].value;
            }
        }
        return _value;
    }

    public int GetIntAttributes(Attributes _attribute)
    {
        int _value =  0;
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i].type == _attribute)
            {
                _value = attributes[i].value.ModifiedValue;
            }
        }
        return _value;
    }

    public ModifiableInt GetStats(Stat _stat)
    {
        ModifiableInt _value = new ModifiableInt();
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i].type == _stat)
            {
                _value = stats[i].value;
            }
        }
        return _value;
    }

    public void SetHP()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i].type == Stat.HP)
            {
                float hpRatio = curruntHP / maxHP;
                stats[i].value.BaseValue = 100 + 10 * (GetIntAttributes(Attributes.Stamina));
                maxHP = stats[i].value.ModifiedValue;
                curruntHP = (int)(maxHP * hpRatio);
            }
        }
    }

    public void SetHeal()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i].type == Stat.Heal)
            {
                stats[i].value.BaseValue = 1 + GetIntAttributes(Attributes.Stamina) + (GetIntAttributes(Attributes.Intellect)/2);
                regenHP = stats[i].value.ModifiedValue;
            }
        }
    }

    public void SetDmg()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i].type == Stat.Attack)
            {
                stats[i].value.BaseValue = 1 + GetIntAttributes(Attributes.Strength) + (GetIntAttributes(Attributes.Agility) / 2);
                attackDamage = stats[i].value.ModifiedValue;
                Debug.Log("attackDamage : " + attackDamage + " BaseVaule : " + stats[i].value.BaseValue + "ModifiedValue" + stats[i].value.ModifiedValue);
            }
        }
        print(string.Concat("Damage: ", attackDamage));
    }

    public int GetIntStats(Stat _stat)
    {
        int _value  = 0;
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i].type == _stat)
            {
                _value = stats[i].value.ModifiedValue;
            }
        }
        return _value;
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
                print(string.Concat("Removed ", _listSlot.BuildObject, " on ", _listSlot.parent.buildList.type, ", Allowed Building: ", string.Join(", ", _listSlot.AllowedBuild)));

                for (int i = 0; i < _listSlot.build.buffs.Length; i++)
                {
                    for (int j = 0; j < stats.Length; j++)
                    {
                        if (stats[j].type == _listSlot.build.buffs[i].stat)
                        {
                            stats[j].value.RemoveModifier(_listSlot.build.buffs[i]);
                        }
                    }
                }
                break;
            case BuildInterfaceList.BuildShop:
                //bm.BuildTownUpdate();
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
                print(string.Concat("Placed ", _listSlot.BuildObject, " on ", _listSlot.parent.buildList.type, ", Allowed Builds: ", string.Join(", ", _listSlot.AllowedBuild)));

                for (int i = 0; i < _listSlot.build.buffs.Length; i++)
                {
                    for (int j = 0; j < stats.Length; j++)
                    {
                        if (stats[j].type == _listSlot.build.buffs[i].stat)
                        {
                            stats[j].value.AddModifier(_listSlot.build.buffs[i]);
                        }
                    }
                }
                UpdatePStats();
                break;
            case BuildInterfaceList.BuildShop:
                //bm.BuildTownUpdate();
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
        if (gm.TempBuildType(BuildType.Store))
        {
            inventory.AddGold(sell.gold);
            InvenGoldUpdate();
            sell.Clear();
        }
        else
        { gm.Alert("You are far away from shop."); }
    }

    public void ShopBuy(int _itemValue, Item _item, int _amount)
    {
        if (gm.TempBuildType(BuildType.Store))
        {
            inventory.RemoveGold(_itemValue);
            inventory.AddItem(_item, _amount);
            InvenGoldUpdate();
        }
        else
        { gm.Alert("You are far away from shop."); }
    }

    public void BuildBuy(int _buildValue, ListSlot _build)
    {
        inventory.RemoveGold(_buildValue);
        buildList.GetListSlots[_build.indexNum].UpdateListSlot(_build.build);
        gm.BuildShopClose();
        bm.ShopListUpdate(_build.indexNum);
        bm.BuildTownUpdate();
        InvenGoldUpdate();
    }
    public void BuildSell(ListSlot _build)
    {
        inventory.AddGold(_build.build.BuildValue);
        buildList.GetListSlots[_build.indexNum].RemoveBuild();
        gm.BuildInfoClose();
        bm.ShopListUpdate(_build.indexNum);
        bm.BuildTownUpdate();
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

    public float HitDamage()
    {
        Debug.Log(attackDamage);
        return attackDamage;
    }

    public void UpdatePStats()
    {
        SetHP();
        SetHeal();
        UpdateHPSlider();
        SetDmg();
        StatDisplayUpdate();
    }

    public void UpdateEXP(int GetExp)
    {
        exp += GetExp;
        if (exp >= (level * 10))
        {
            exp -= level * 10;
            level++;
            lvText.text = "LV : " + level.ToString("n0");
        }
        expText.text = exp.ToString("n0") + " / " + level * 10;
        expSlider.value = exp / level;
    }

    public void StatDisplayUpdate()
    {
        string att ="";
        for (int i = 0; i < attributes.Length; i++)
        {
            ModifiableInt _value = GetAttributes(attributes[i].type);
            att += "\n" + attributes[i].type + " : " + _value.ModifiedValue.ToString("n0") + " (" + _value.BaseValue.ToString("n0") + " + " + (_value.ModifiedValue - _value.BaseValue).ToString("n0") + ")";
        }
        attText.text = "Attributes\n" + att;

        string _stat = "";
        for (int i = 0; i < stats.Length; i++)
        {
            ModifiableInt _value = GetStats(stats[i].type);
            _stat += "\n" + stats[i].type + " : " + _value.ModifiedValue.ToString("n0") + " (" + _value.BaseValue.ToString("n0") + " + " + (_value.ModifiedValue - _value.BaseValue).ToString("n0") + ")";
        }
        statText.text = "Stats\n" + _stat;
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
        if (curruntHP >= maxHP)
        { curruntHP = maxHP; }
        HPSlider.value = curruntHP / maxHP;
        gm.HPTextUpdate(curruntHP, maxHP);
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    public void StatModified(PlayerStat stat)
    {
        Debug.Log(string.Concat(stat.type, " was updated! Value is now ", stat.value.ModifiedValue));
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

[System.Serializable]
public class PlayerStat
{
    [System.NonSerialized]
    public Player parent;
    public Stat type;
    public ModifiableInt value;

    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(StatModified);
    }
    public void StatModified()
    {
        parent.StatModified(this);
    }
}
using StarterAssets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Stat
{
    HP,
    Heal,
    Attack,
    AttackSpeed,
    MoveSpeed,
    Armor,
    MoneyEarn
}


public class Player : MonoBehaviour
{
    public string savePath;
    public InventoryObject inven;
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
    public Animator anim;
    public InventoryObject tutorial;

    public Attribute[] attributes;
    public PlayerStat[] stats;
    public PlayerLV lv;

    public float maxHP = 100;
    public float curruntHP;
    public float attackDamage;
    public float regenHP = 0;
    public float armor = 0;
    public float attackSpeed = 0.5f;
    public float moveSpeed = 5.0f;
    public float moneyEarn = 1.00f;

    public Dictionary<Stat, int> StatOnIndex = new Dictionary<Stat, int>();

    GameManager gm;
    BuildManager bm;
    TextMeshProUGUI invenTextGold;
    TextMeshProUGUI sellTextGold;
    ThirdPersonController tpc;

    private void Awake()
    {
        
    }

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        bm = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        tpc = GetComponentInParent<ThirdPersonController>();
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].SetParent(this);
            StatOnIndex.Add(stats[i].type, i);
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
                case Stat.Armor:
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

        curruntHP = maxHP = 100;
        LvStatUp();
        UpdateEXP();
        invenTextGold = invenGoldObj.GetComponent<TextMeshProUGUI>();
        sellTextGold = sellGoldObj.GetComponent<TextMeshProUGUI>();

        //update BD ID;
        inven.database.UpdateID();
        tutorial.database.UpdateID();
        buildList.database.UpdateID();

        //StartCoroutine(CheckSave());
    }

    IEnumerator CheckSave()
    {
        yield return new WaitForSeconds(1f);
        int _saveSlot = PlayerPrefs.GetInt("UseSlot");
        //Load data.
        if (_saveSlot <= 3 && _saveSlot >= 1)
        {
            gm.LoadInventory(_saveSlot);
            Debug.Log(_saveSlot);
        }
        else if (_saveSlot == 0)
        {
            gm.LoadInventory();
            Debug.Log(_saveSlot);
        }
        else if (_saveSlot <= -1 && _saveSlot >= -3)
        {
            //Add Basic Items
            /*for (int i = 0; i < tutorial.GetSlots.Length; i++)
            {
                if (tutorial.AddItem(new Item(tutorial.database.ItemObjects[i]), 1))
                { }
                if (inven.AddItem(new Item(tutorial.database.ItemObjects[i]), 1))
                { }
            }*/
            //gm.SaveInventory(-(_saveSlot));
            Debug.Log(_saveSlot);
        }
    }

    private void Update()
    {
        if (curruntHP < maxHP)
        {
            curruntHP += (regenHP * Time.deltaTime / 10);
            UpdateHPSlider();
        }
    }

    public int FindStatIndex(Stat _stat)
    {
        if (StatOnIndex.ContainsKey(_stat))
        {
            return StatOnIndex[_stat];
        }
        else { return -1; }
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

    public void SetHP()
    {
        int _hp = FindStatIndex(Stat.HP);
        if (_hp != -1)
        {
            float hpRatio = curruntHP / maxHP;
            stats[_hp].value.BaseValue = 100 + 10 * (GetIntAttributes(Attributes.Stamina));
            maxHP = stats[_hp].value.ModifiedValue;
            curruntHP = (int)(maxHP * hpRatio);
        }
    }

    public void SetHeal()
    {
        int _heal = FindStatIndex(Stat.Heal);
        if (_heal != -1)
        {
            stats[_heal].value.BaseValue = 1 + GetIntAttributes(Attributes.Stamina) + (GetIntAttributes(Attributes.Intellect)/2);
            regenHP = stats[_heal].value.ModifiedValue;
        }
    }

    public void SetDmg()
    {
        int _attack = FindStatIndex(Stat.Attack);
        if(_attack != -1)
        {
            stats[_attack].value.BaseValue = 1 + GetIntAttributes(Attributes.Strength) + (GetIntAttributes(Attributes.Intellect) / 2);
            attackDamage = stats[_attack].value.ModifiedValue;
            Debug.Log("attackDamage: " + attackDamage + " BaseVaule: " + stats[_attack].value.BaseValue);
        }
    }

    public void SetAtkSpeed()
    {
        int _atkSpd = FindStatIndex(Stat.AttackSpeed);
        if (_atkSpd != -1)
        {
            stats[_atkSpd].value.BaseValue = 0.5f + ((float)GetIntAttributes(Attributes.Agility) / 10);
            attackSpeed = stats[_atkSpd ].value.ModifiedValue;
            tpc.AttackTimeout = 1.0f / attackSpeed;
            float animAtkSpd = attackSpeed * 2.0f;
            anim.SetFloat("AttackSpeed", animAtkSpd);
        }
    }

    public void SetMoveSpeed()
    {
        int _mvSpd = FindStatIndex(Stat.MoveSpeed);
        if (_mvSpd != -1)
        {
            stats[_mvSpd].value.BaseValue = 5.0f + ((float)GetIntAttributes(Attributes.Agility) / 10);
            moveSpeed = stats[_mvSpd ].value.ModifiedValue;
            tpc.MoveSpeed = moveSpeed;
            tpc.SprintSpeed = moveSpeed * 2;
            float moveSpd = moveSpeed * 0.2f;
            anim.SetFloat("MoveSpeed", moveSpd);
        }
    }

    public void SetArmor()
    {
        int _armor = FindStatIndex(Stat.Armor);
        if (_armor != -1)
        {
            stats[_armor].value.BaseValue = (GetIntAttributes(Attributes.Intellect)) / 2;
            armor = stats[_armor].value.ModifiedValue;
        }
    }

    public void SetMoneyEarn()
    {
        int _money = FindStatIndex(Stat.MoneyEarn);
        if (_money != -1)
        {
            moneyEarn = stats[_money].value.ModifiedValue;
        }
    }


    public void InvenGoldUpdate()
    {
        invenTextGold.text = inven.GetGold.ToString("n0") + " G";
    }

    public void SellGoldUpdate()
    {
        sellTextGold.text = sell.GetGold.ToString("n0") + " G";
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
                UpdatePStats();
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
        if (sell.OnSlotCount > inven.EmptySlotCount)
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
                if (inven.AddItem(_item, _amount))
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
            inven.AddGold(sell.GetGold);
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
            inven.RemoveGold(_itemValue);
            inven.AddItem(_item, _amount);
            InvenGoldUpdate();
        }
        else
        { gm.Alert("You are far away from shop."); }
    }

    public void BuildBuy(int _buildValue, ListSlot _build)
    {
        inven.RemoveGold(_buildValue);
        buildList.GetListSlots[_build.indexNum].UpdateListSlot(_build.build);
        gm.BuildShopClose();
        bm.ShopListUpdate(_build.indexNum);
        bm.BuildTownUpdate();
        InvenGoldUpdate();
    }
    public void BuildSell(ListSlot _build)
    {
        inven.AddGold(_build.build.BuildValue);
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
        inven.UseItem(_itemObj.data);
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
        SetAtkSpeed();
        SetMoveSpeed();
        SetArmor();
        StatDisplayUpdate();
    }

    public void UpdateEXP(int GetExp)
    {
        lv.exp += GetExp;
        LvUpCheck();
        expText.text = lv.exp.ToString("n0") + " / " + lv.level * 10;
        expSlider.value = lv.exp / lv.level;
    }

    public void UpdateEXP()
    {
        expText.text = lv.exp.ToString("n0") + " / " + lv.level * 10;
        expSlider.value = lv.exp / lv.level;
    }

    private void LvUpCheck()
    {
        if (lv.exp >= (lv.level * 10))
        {
            lv.exp -= lv.level * 10;
            lv.level++;
            lvText.text = "LV : " + lv.level.ToString("n0");
            LvStatUp();
            LvUpCheck();
        }
    }

    private void LvStatUp()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].value.BaseValue = lv.level;
            UpdatePStats();
        }
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
            ModifiableFloat _value = stats[i].value;
            _stat += "\n" + stats[i].type + " : " + _value.ModifiedValue.ToString("f1") + " (" + _value.BaseValue.ToString("f1") + " + " + (_value.ModifiedValue - _value.BaseValue).ToString("f1") + ")";
        }
        statText.text = "Stats\n" + _stat;
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            if (inven.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void GetDamaged(float damage)
    {
        damage -= armor;
        if(damage < 0)
        { damage = 0; }
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

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, lv);
        stream.Close();
    }

    public void Save(int _saveSlot)
    {

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + _saveSlot), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, lv);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            PlayerLV newLV = (PlayerLV)formatter.Deserialize(stream);
            lv.level = newLV.level;
            lv.exp = newLV.exp;
            stream.Close();
            UpdateEXP();
            LvStatUp();
        }
    }

    public void Load(int _saveSlot)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + _saveSlot)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + _saveSlot), FileMode.Open, FileAccess.Read);
            PlayerLV newLV = (PlayerLV)formatter.Deserialize(stream);
            lv.level = newLV.level;
            lv.exp = newLV.exp;
            stream.Close();
            UpdateEXP();
            LvStatUp();
        }
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
    public ModifiableFloat value;

    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableFloat(StatModified);
    }
    public void StatModified()
    {
        parent.StatModified(this);
    }
}

[System.Serializable]
public class PlayerLV
{
    public int level = 1;
    public int exp = 0;
}
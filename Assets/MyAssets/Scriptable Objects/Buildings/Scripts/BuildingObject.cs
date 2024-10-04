using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum BuildType
{
    House,
    Farm,
    Store,
    Smith,
    Stable,
    AnimalFarm,
    Tarvern,
    Castle,
    Church,
    Windmill
}

[CreateAssetMenu(fileName = "New Building", menuName = "Inventory System/Buildings/Building")]
public class BuildingObject : ScriptableObject
{
    public Sprite uiDisplay;
    public Mesh buildingMesh;
    public BuildType type;
    [TextArea(15, 20)]
    public string description;
    public Building data = new Building();

    public Building CreateBuilding()
    {
        Building newBuild = new Building(this);
        return newBuild;
    }
}

[System.Serializable]
public class Building
{
    public string BuildName;
    public int B_Id = -1;
    public int BuildValue = 0;
    public BuildBuff[] buffs;

    public Building()
    {
        BuildName = "";
        B_Id = -1;
    }

    public Building(BuildingObject buliding)
    {
        BuildName = buliding.name;
        B_Id = buliding.data.B_Id;
        BuildValue = buliding.data.BuildValue;
        buffs = new BuildBuff[buliding.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new BuildBuff(buliding.data.buffs[i].buffvalue);
            {
                buffs[i].attribute = buliding.data.buffs[i].attribute;
                BuildValue += buffs[i].buffvalue;
            };
        }
    }
}

[System.Serializable]
public class BuildBuff : IModifiers
{
    public Attributes attribute;
    public int buffvalue;
    public int min;
    public int max;

    public BuildBuff(int _buffValue)
    {
        buffvalue = _buffValue;
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += buffvalue;
    }
}
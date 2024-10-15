using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum BuildType
{
    Default,
    House,//House2,
    Farm,
    Store,
    Smith,
    Stable,
    AnimalFarm,
    Tarvern,
    Castle,
    Church,
    Windmill,
    Wall
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
    public int Id = -1;
    public int BuildValue = 0;
    public int BuildLevel = 0;
    public BuildType type;
    public BuildBuff[] buffs;
    public string buffList = "";

    public Building()
    {
        BuildName = "";
        Id = -1;
    }

    public Building(BuildingObject building)
    {
        BuildName = building.name;
        Id = building.data.Id;
        BuildValue = building.data.BuildValue;
        BuildLevel = building.data.BuildLevel;
        type = building.data.type;
        buffList = building.data.buffList;
        buffs = new BuildBuff[building.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new BuildBuff(building.data.buffs[i].buffvalue);
            {
                buffs[i].attribute = building.data.buffs[i].attribute;
                buffList += buffs[i].attribute.ToString() + ": " + buffs[i].buffvalue.ToString("n0") + "\n";
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
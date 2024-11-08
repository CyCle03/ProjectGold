using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using JetBrains.Annotations;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;

public enum BuildInterfaceList
{
    BuildList,
    BuildShop,
    ETC
}

[CreateAssetMenu(fileName = "New BuildList", menuName = "Inventory System/BuildList")]
public class BuildingListObject : ScriptableObject
{
    public string savePath;
    public BuildingDBObject database;
    public BuildInterfaceList type;
    public BuildList Container;
    public ListSlot[] GetListSlots { get { return Container.ListSlots; } }
    public int gold = 0;

    public bool AddBulid(Building _build)
    {
        if (EmptyListCount <= 0)
        {
            return false;
        }

        ListSlot listslot = FindBuildOnList(_build);

        if (listslot == null)
        {
            return false;
        }
        listslot.UpdateListSlot(_build);
        return true;
    }

    public int EmptyListCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetListSlots.Length; i++)
            {
                if (GetListSlots[i].build.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public int OnListSlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetListSlots.Length; i++)
            {
                if (GetListSlots[i].build.Id >= 0)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public ListSlot FindBuildOnList(Building _build)
    {
        for (int i = 0; i < GetListSlots.Length; i++)
        {
            if (GetListSlots[i].CanPlaceInListSlot(_build))
            {
                if (GetListSlots[i].build.Id <= -1)
                {
                    return GetListSlots[i];
                }
            }
        }
        return null;
    }

    public ListSlot IsBuildOnList(BuildingObject _build)
    {
        for (int i = 0; i < GetListSlots.Length; i++)
        {
            if (GetListSlots[i].CanPlaceInListSlot(_build) && GetListSlots[i].build.Id == _build.data.Id)
            {
                return GetListSlots[i];
            }
        }
        return null;
    }


    public ListSlot SetEmptyListSlot(Building _build)
    {
        for (int i = 0; i < GetListSlots.Length; i++)
        {
            if (GetListSlots[i].build.Id <= -1)
            {
                GetListSlots[i].UpdateListSlot(_build);
                return GetListSlots[i];
            }
        }

        //set up funcionallity for full inventory
        return null;
    }

    public void SwapBuilds(ListSlot build1, ListSlot build2)
    {
        if (build2.CanPlaceInListSlot(build1.BuildObject) && build1.CanPlaceInListSlot(build2.BuildObject))
        {
            ListSlot temp = new ListSlot(build2.build);
            build2.UpdateListSlot(build1.build);
            build1.UpdateListSlot(temp.build);
        }
    }

    public void RemoveBuild(Building _build)
    {
        for (int i = 0; i < GetListSlots.Length; i++)
        {
            if (GetListSlots[i].build == _build)
            {
                GetListSlots[i].RemoveBuild();
            }
        }
    }

    public void AddGold(int _gold)
    {
        gold += _gold;
    }

    public void RemoveGold(int _gold)
    {
        gold -= _gold;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize (stream, Container);
        stream.Close();
    }

    public void Save(int _saveSlot)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + _saveSlot), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            BuildList newContainer = (BuildList)formatter.Deserialize(stream);
            for (int i = 0; i < GetListSlots.Length; i++)
            {
                GetListSlots[i].UpdateListSlot(newContainer.ListSlots[i].build);
            }
            stream.Close ();
        }
    }

    public void Load(int _saveSlot)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + _saveSlot)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath + _saveSlot), FileMode.Open, FileAccess.Read);
            BuildList newContainer = (BuildList)formatter.Deserialize(stream);
            for (int i = 0; i < GetListSlots.Length; i++)
            {
                GetListSlots[i].UpdateListSlot(newContainer.ListSlots[i].build);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
        gold = 0;
    }

}

[System.Serializable]
public class BuildList
{
    public ListSlot[] ListSlots = new ListSlot[14];
    public void Clear()
    {
        for (int i = 0; i < ListSlots.Length; i++)
        {
            ListSlots[i].RemoveBuild();
        }
    }
}

public delegate void ListSlotUpdated(ListSlot _slot);

[System.Serializable]
public class ListSlot
{
    public BuildType AllowedBuild = new BuildType();

    [System.NonSerialized]
    public BuildInterface parent;
    [System.NonSerialized]
    public GameObject listSlotDisplay;
    [System.NonSerialized]
    public ListSlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public ListSlotUpdated OnBeforeUpdate;
    [System.NonSerialized]
    public int indexNum;

    public Building build;

    public BuildingObject BuildObject
    {
        get
        {
            if (build.Id >= 0)
            {
                return parent.buildList.database.BuildObjects[build.Id];
            }
            return null;
        }
    }

    public ListSlot()
    {
        UpdateListSlot(new Building());
    }

    public ListSlot(Building _build)
    {
        UpdateListSlot(_build);
    }

    public void UpdateListSlot(Building _build)
    {
        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }
        build = _build;
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    public void RemoveBuild()
    {
        UpdateListSlot(new Building());
    }

    public bool CanPlaceInListSlot(BuildingObject _buildObject)
    {
        if (AllowedBuild == BuildType.Default || _buildObject == null || _buildObject.data.Id < 0)
        {
            return true;
        }
        if (_buildObject.type == AllowedBuild)
        {
            return true;
        }
        return false;
    }

    public bool CanPlaceInListSlot(Building _build)
    {
        if (AllowedBuild == BuildType.Default || _build == null || _build.Id < 0)
        {
            return true;
        }
        if (_build.type == AllowedBuild)
        {
            return true;
        }
        return false;
    }
}
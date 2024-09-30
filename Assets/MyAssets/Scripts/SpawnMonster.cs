using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnMonster : MonoBehaviour, ISerializationCallbackReceiver
{
    public MonsterObject monster;

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        //GetComponentInChildren<MeshRenderer>(). = monster.monsterObj;
        //EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }
}

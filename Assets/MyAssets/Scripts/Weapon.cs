using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum WeaponType
{
    Melee,
    Range,
    Magic,
    Knife
}*/

public class Weapon : MonoBehaviour
{
    public Player player;
    //public WeaponType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponent<MonsterMove>();
        if (monster != null)
        {
            monster.GetDamaged(player.HitDamage());
            gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}

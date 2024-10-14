using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInteract : MonoBehaviour
{
    public Building myBuild;
    public BuildingObject buildObj;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        myBuild = new Building(buildObj);
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.GetComponent<Player>();

        if (otherPlayer != null)
        { gm.InteractBuild(myBuild); }
    }

    private void OnTriggerExit(Collider other)
    {
        var otherPlayer = other.GetComponent<Player>();
        if (otherPlayer != null)
        { gm.InteractBuild(); }
    }
}

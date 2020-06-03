using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Author: Roger Clanton
 * 
 * Controls the list of building that are placeable, used by BuildingPlacer.cs
 * Has to be updated everytime a new building type is added.
 * 
 * TODO:
 * Integrate with the UI
 */
public class Building : MonoBehaviour
{
    ulong uID;

    public GameObject buildingPrefab;
    public GameObject buildingPrefab1;
    public GameObject buildingPrefab2;
    public GameObject buildingPrefab3;
    public GameObject buildingPrefab4;

    public Player player;


    public int bldInt = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        buildingPrefab = buildingPrefab1;
        bldInt = 1;
    }

    private void Update()
    {

    }

    public int getBldType()
    {
        return bldInt;
    }

    public GameObject getBuildingPrefab()
    {
        return buildingPrefab;
    }

    public GameObject switchBuilding(int x)
    {
        switch (x)
        {
            case 1:  buildingPrefab = buildingPrefab1;
                bldInt = 1;
                break;
            case 2:
                bldInt = 2;
                buildingPrefab = buildingPrefab2;
                break;
            case 3:
                bldInt = 3;
                buildingPrefab = buildingPrefab3;
                break;
            case 4:
                bldInt = 4;
                buildingPrefab = buildingPrefab4;
                break;
        }
        return buildingPrefab;
    }

    //Calls the placement function for each building type
    public void place(GameObject placement)
    {
        switch (bldInt)
        {
            case 1:
                placement.GetComponent<EnergyProducer>().place(player);
                break;
            case 2:
                placement.GetComponent<Pylon>().place();
                break;
            case 3:
                placement.GetComponent<Producer>().place();
                break;
            case 4:
                placement.GetComponent<Factory>().place(player);
                break;
        }
    }

    public void givePlayer(Player p)
    {
        player = p;
    }
}

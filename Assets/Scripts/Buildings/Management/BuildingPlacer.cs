﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * Author: Roger Clanton
 * 
 * Places buildings through the UI
 * 
 * TODO:
 * Add new building and placement types as we create them
 * Integrate with the UI
 */
public class BuildingPlacer : MonoBehaviour
{

    public GridR grid;
    public Player player;
    public GameObject gui;

    public Material placeable;
    public Material unplaceable;
    public Material notConstructed;
    public Material finishedConstructing;

    private GameObject factoryPrefab;

    public LayerMask placementLayerMask;
    public LayerMask buildingsLayerMask;


    private BuildingController building;


    

    public GridR getGrid()
    {
        return grid;
    }
    public bool checkPosition(GameObject cursor)
    {
        //plus 5 comes from the offset
        int bldType = grid.getBuilding(cursor.transform.position.x, cursor.transform.position.z);
        SortedSet<int> validTypes = GetValidFoundations(cursor);
        if (validTypes.Contains(bldType))
        {
            
            foreach (Transform child in cursor.transform)
            {
                if (!validTypes.Contains(grid.getBuilding(child.position.x, child.position.z)))
                {
                    return false;
                }
            }
        }
        else { return false; }
        return true;
    }

    private SortedSet<int> GetValidFoundations(GameObject cursor)
    {
        if (cursor.TryGetComponent<EnergyProducer>(out EnergyProducer energy))
        {
            return energy.getValidFoundation();
        }
        if (cursor.TryGetComponent<Pylon>(out Pylon pylon))
        {
            return pylon.getValidFoundation();
        }
        if (cursor.TryGetComponent<MassProducer>(out MassProducer producer))
        {
            return producer.getValidFoundation();
        }
        if (cursor.TryGetComponent<Factory>(out Factory factory))
        {
            return factory.getValidFoundation();
        }
        return new SortedSet<int> { };
    }
    
    public void PlaceBuildingNear(GameObject cursor){
        //Place the object based on the normal
        GameObject build = GameObject.Instantiate(cursor.GetComponent<BuildingController>().getBuildingPrefab());
        
        build.transform.position = cursor.transform.position;
        build.transform.rotation = cursor.transform.rotation;

        //update the grid
        int bldInt = build.GetComponent<BuildingController>().getBldType();
        grid.updateBuilding(build.transform.position.x, build.transform.position.z, bldInt);
        foreach (Transform child in cursor.transform)
        {
            grid.updateBuilding(child.position.x, child.position.z, bldInt);
            //building.place(build);
        }
        build.GetComponent<BuildingController>().Place(player,gui);
    }

    public GameObject getFactoryPrefab()
    {
        return factoryPrefab;
    }

}

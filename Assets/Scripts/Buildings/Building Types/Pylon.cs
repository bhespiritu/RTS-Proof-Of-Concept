using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pylon : MonoBehaviour
{
    private int buffGiven = 1;
    private int cost;
    private int costPT;
    private int energyCostPT;
    private int energyCostTotal;
    private int bldInt = 6;

    BuildingPlacer buildPlacer;
    GridR grid;
    private SortedSet<int> placeable = new SortedSet<int>{ -1 };

    // Start is called before the first frame update
    void Start()
    {
        buildPlacer = FindObjectOfType<BuildingPlacer>();
        grid = buildPlacer.GetComponent<GridR>();

    }

    // Update is called once per frame
    public int GetBldInt()
    {
        return bldInt;
    }

    //Should return a cost to build this unit.
    public void place()
    {
        // use for if we need to do anything when pylons are placed.
    }
    public int getBuff()
    {
        return buffGiven;
    }

    public SortedSet<int> getValidFoundation()
    {
        return placeable;
    }

    public int getCost()
    {
        return cost;
    }
    public int getCostPT()
    {
        return costPT;
    }

    public int getEnergyCost()
    {
        return energyCostTotal;
    }
    public int getEnergyCostPT()
    {
        return energyCostPT;
    }

    internal GameObject getBuildingPrefab()
    {
        throw new NotImplementedException();
    }
}

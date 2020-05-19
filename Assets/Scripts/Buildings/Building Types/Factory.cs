using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private int cost;
    private int costPS;
    private int energyCostPS;
    private int energyCostTotal;
    private int unitTotalCost = 0;
    private int unitCostPerFrame = 0;
    public GameObject unit1;
    public Player player;
    public int health;

    BuildingPlacer buildPlacer;
    GridR grid;
    private SortedSet<int> placeable = new SortedSet<int> { -1 };

    // Start is called before the first frame update
    void Start()
    {
        buildPlacer = FindObjectOfType<BuildingPlacer>();
        grid = buildPlacer.GetComponent<GridR>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Should return a cost to build this unit.
    public void place(Player p)
    {
        player = p;
        // use for if we need to do anything when pylons are placed.
    }

    public SortedSet<int> getValidFoundation()
    {
        return placeable;
    }

    public int getCost()
    {
        return cost;
    }
    public int getCostPS()
    {
        return costPS;
    }

    public int getEnergyCost()
    {
        return energyCostTotal;
    }
    public int getEnergyCostPS()
    {
        return energyCostPS;
    }// Start is called before the first frame update
    
    //Probably will take in a unit type as an argument
    public void build()
    {
        //Get the unit cost
        
        //Set the build percentage to 0
    }

    /// <summary>
    /// called each frame to request energy from the player and to calculate the amount of work done
    /// </summary>
    public void work(float e)
    {
        
    }
}

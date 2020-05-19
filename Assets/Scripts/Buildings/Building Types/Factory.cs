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
    public Unit unit;

    public int health;

    [SerializeField]
    public Requester request;

    private bool producing = false;
    private int progress;

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
    void FixedUpdate()
    {
        if (producing)
        {
            //Request Energy
            player.energyRequest(request,energyCostPS);
        }
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
    }
    // Start is called before the first frame update
    
    //Probably will take in a unit type as an argument
    public void build()
    {
        producing = true;
        //Get the unit cost

        //Replace with actual stuff
        energyCostTotal = 500;
        energyCostPS = 1;

        //Set the build percentage to 0
        progress = 0;

    }

    /// <summary>
    /// called each frame to request energy from the player and to calculate the amount of work done
    /// </summary>
    public void work(float e)
    {

        if(e * energyCostPS + progress >= 100)
        {
            player.recieveEnergy((int)e * energyCostPS + progress - 100);
            finish();
        }
        else
        {
            progress += (int)(e * energyCostPS);
        }
    }
    private void finish()
    {
        producing = false;
        
    }
}

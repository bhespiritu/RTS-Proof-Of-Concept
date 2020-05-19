using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private int bld = 5;
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
    private bool isPlaced = false;
    private int progress;

    BuildingPlacer buildPlacer;
    GridR grid;

    private SortedSet<int> placeable = new SortedSet<int> { -1 };

    // Start is called before the first frame update
    void Start()
    {
        buildPlacer = FindObjectOfType<BuildingPlacer>();
        grid = buildPlacer.GetComponent<GridR>();
        request = gameObject.GetComponent<Requester>();
        build();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            if (producing)
            {
                //Request Energy
                Debug.Log("Requesting Energy");
                player.energyRequest(request, energyCostPS);
            }
        }
    }

    //Should return a cost to build this unit.
    public void place(Player p)
    {
        isPlaced = true;
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
        Debug.Log("Working...: " + progress);
        Debug.Log("E is : " + e);
        if (e * energyCostPS + progress >= energyCostTotal)
        {
            player.recieveEnergy((int)e * energyCostPS + progress - energyCostTotal);
            finish();
        }
        else
        {
            progress += (int)(e * energyCostPS);
        }
    }
    private void finish()
    {
        Debug.Log("Finish");
        producing = false;
        GameObject unit = Instantiate(unit1, transform.position + new Vector3(10, 10, 10), Quaternion.identity);
        unit.AddComponent<Unit>();
    }
}

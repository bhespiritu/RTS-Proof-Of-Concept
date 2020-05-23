using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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

    public Player player;


    public GameObject unit1;
    public Unit unit;
    private Unit unitConstructing;

    //What will currently be built
    private List<Unit> orders;
    //Used so a factory will repeatedly build a group of units
    private List<Unit> savedOrders;
    //Used to toggle whether a set of orders should be repeated
    private bool repeat = false;

    private MeshRenderer mesh;
    private MeshRenderer childMesh;
    private Material notConstructed;
    private Material finishedConstructing;

    public int health;

    [SerializeField]
    public Requester request;

    private bool producing = false;
    private bool isPlaced = false;
    private int progress;
    private bool constructed = false;

    BuildingPlacer buildPlacer;
    GridR grid;

    private SortedSet<int> placeable = new SortedSet<int> { -1 };

    // Start is called before the first frame update
    void Start()
    {
        buildPlacer = FindObjectOfType<BuildingPlacer>();
        grid = buildPlacer.GetComponent<GridR>();
        request = gameObject.GetComponent<Requester>();
        mesh = gameObject.GetComponent<MeshRenderer>();
        //Todo make this a method
        notConstructed = buildPlacer.notConstructed;
        finishedConstructing = buildPlacer.finishedConstructing;
        changeMesh(notConstructed);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            if (producing)
            {
                Debug.Log("Factory requests energy for unit: " + energyCostPS);
                //Request Energy
                player.energyRequest(request, energyCostPS);
            }
            else if (!constructed)
            {
                Debug.Log("Factory requests energy for self: " + energyCostPS);
                player.energyRequest(request, energyCostPS);
            }
            
        }
    }

    //Should return a cost to build this unit.
    public void place(Player p)
    {
        isPlaced = true;
        player = p;
        buildSelf();
        
    }

    /// <summary>
    /// Make the current build queue be repeated by the factory
    /// </summary>
    public void repeatOrder()
    {
        repeat = true;
        savedOrders = deepCopy(orders);
    }

    public void Order(Unit u)
    {
        orders.Add(u);
    }

    public void removeOrder(Unit u)
    {
        orders.Remove(u);
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
 
    //Used when the factory is being constructed
    public void buildSelf()
    {
        energyCostTotal = 10000;
        energyCostPS = 50;
        progress = 0;
        orders = new List<Unit>();
        savedOrders = new List<Unit>();
    }

    public void toggleProduction()
    {
        producing = !producing;
    }

    public void build()
    {
        unitConstructing = orders[0];
        orders.RemoveAt(0);
        producing = true;
        //Get the unit cost

        //Replace with actual stuff
        // energyCostTotal = unitConstructing.getEnergyCost();
        // energyCostPS = unitConstructing.getEnergyCostPS();
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
        if (constructed)
        {
            //If there are no more orders in the queue
            if (orders.Count == 0)
            {
                
                GameObject unit = Instantiate(unit1, transform.position + new Vector3(10, 10, 10), Quaternion.identity);
                unit.AddComponent<Unit>();
                if (repeat)
                {
                    orders = deepCopy(savedOrders);
                    build();
                }

                else{ producing = false; }

            }

            else {
                build();
            }

        }

        else
        {
            constructed = true; 
            changeMesh(finishedConstructing);
            //Test line to automatically build a unit
            Order(unit1.GetComponent<Unit>());
            if (orders.Count != 0)
            {
                Debug.Log("Make a unit");
                build();
            }
            
        }
        Debug.Log("Finish");
        
    }
    private void changeMesh(Material m)
    {
        Debug.Log(m);
        mesh.material = m;
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = 8;
            childMesh = child.GetComponent<MeshRenderer>();
            childMesh.material = m;
        }
    }

    private List<Unit> deepCopy(List<Unit> q)
    {
        List<Unit> retQ = new List<Unit>();
        if (q.Count != 0)
        {
            Unit[] units = q.ToArray();
            foreach(Unit u in units)
            {
                retQ.Add(u);
            }
            return retQ;
        }
        return retQ;
    }
}

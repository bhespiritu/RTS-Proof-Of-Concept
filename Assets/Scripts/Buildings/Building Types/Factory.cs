using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

/*
* Author: Roger Clanton:
* 
* A basic factory
* 
* TODO:
* Add upgradability
* integrate with UI
* test if order queueing works
* Remove Per second and make Per tick
*/

public class Factory : MonoBehaviour
{
    //private int bld = 5;
    private int cost ;
    private int costPS;
    private int energyCostPT;
    private int energyCostTotal;


    public Player player;

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
        RoundTimeManager.OnRoundTick += onTick;
        
    }

    // Update is called once per frame
    void onTick()
    {
        if (isPlaced)
        {
            if (producing)
            {
                Debug.Log("Factory requests energy for unit: " + energyCostPT);
                //Request Energy
                player.energyRequest(request, energyCostPT);
            }
            else if (!constructed)
            {
                Debug.Log("Factory requests energy for self: " + energyCostPT);
                player.energyRequest(request, energyCostPT);
            }
            
        }
    }

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
        return energyCostPT;
    }


    public void toggleProduction()
    {
        producing = !producing;
    }

    //Used when the factory is being constructed
    public void buildSelf()
    {
        energyCostTotal = 10000;
        energyCostPT = 50;
        progress = 0;
        orders = new List<Unit>();
        savedOrders = new List<Unit>();
    }

    public void build()
    {
        Debug.Log("Make a unit");
        unitConstructing = orders[0];
        orders.RemoveAt(0);
        producing = true;

        energyCostTotal = unitConstructing.getECTot();
        energyCostPT = unitConstructing.getMCPT();

        //Set the build percentage to 0
        progress = 0;

    }

    /// <summary>
    /// called each frame to request energy from the player and to calculate the amount of work done
    /// </summary>
    public void work(float e)
    {
        if (e * energyCostPT + progress >= energyCostTotal)
        {
            player.recieveEnergy((int)e * energyCostPT + progress - energyCostTotal);
            finish();
        }
        else
        {
            progress += (int)(e * energyCostPT);
        }
    }

    private void finish()
    {
        if (constructed)
        {
            GameObject builtUnit = Instantiate(unit.gameObject, transform.position + new Vector3(10, 10, 10), Quaternion.identity);
            builtUnit.AddComponent<Unit>();
            //If there are no more orders in the queue
            if (orders.Count == 0)
            {
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
            //Test lines to automatically build a unit
            Order(unit);
            if (orders.Count != 0)
            {
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

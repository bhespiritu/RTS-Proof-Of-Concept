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

    public GameObject unit1;
    public Player player;
    public Unit unit;


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
        buildSelf();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            if (!constructed)
            {
                Debug.Log("Factory requests energy for self: " + energyCostPS);
                player.energyRequest(request, energyCostPS);
            }
            if (producing)
            {
                Debug.Log("Factory requests energy for unit: " + energyCostPS);
                //Request Energy
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
 
    //Used when the factory is being constructed
    public void buildSelf()
    {
        energyCostTotal = 10000;
        energyCostPS = 50;
        progress = 0;
    }
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
            producing = false;
            GameObject unit = Instantiate(unit1, transform.position + new Vector3(10, 10, 10), Quaternion.identity);
            unit.AddComponent<Unit>();
        }
        else
        {
            constructed = true; 
            changeMesh(finishedConstructing);
            build();
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
}

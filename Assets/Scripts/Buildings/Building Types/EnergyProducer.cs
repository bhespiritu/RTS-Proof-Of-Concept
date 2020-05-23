using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Author: Roger Clanton
* 
* A building that produces energy
* 
* TODO
* Update to factory standards:
* Make it use RoundTimeManager
* Integrate with Player.cs
* Give it cost attributes and health
*/

public class EnergyProducer : MonoBehaviour
{
    private int eps = 100;
    private int bld = 4;
    private SortedSet<int> placeable = new SortedSet<int> { -1 };
    //Buff is the number of buffing buildings
    private int buff = 0;
    BuildingPlacer buildPlacer;
    private GridR grid;
    private Player player;
    public bool isPlaced = false;
    // Start is called before the first frame update
    void Start()
    {
        buildPlacer = FindObjectOfType<BuildingPlacer>();
        grid = buildPlacer.getGrid();
    }

    public void place(Player p)
    {
        player = p;
        isPlaced = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            buff = checkAdj();
            player.recieveEnergy(production());
        }
    }

    public int getBuildingType()
    {
        return bld;
    }

    public int checkAdj()
    {
        //reset the buff in case nearby building have died
        int newBuff = 0;
        newBuff += checkBonus((int)this.transform.position.x - (int)grid.getSpacing(), (int)this.transform.position.z);
        newBuff += checkBonus((int)this.transform.position.x - (int)grid.getSpacing(), (int)this.transform.position.z - (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x - (int)grid.getSpacing(), (int)this.transform.position.z + (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x - (int)grid.getSpacing(), (int)this.transform.position.z + 2 * (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x, (int)this.transform.position.z + 2 * (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x + (int)grid.getSpacing(), (int)this.transform.position.z + 2 * (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x + 2 * (int)grid.getSpacing(), (int)this.transform.position.z + 2 * (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x + 2 * (int)grid.getSpacing(), (int)this.transform.position.z + (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x + 2 * (int)grid.getSpacing(), (int)this.transform.position.z);
        newBuff += checkBonus((int)this.transform.position.x + 2 * (int)grid.getSpacing(), (int)this.transform.position.z - (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x + (int)grid.getSpacing(), (int)this.transform.position.z - (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x, (int)this.transform.position.z - (int)grid.getSpacing());
        return newBuff;
    }

    private int checkBonus(int x, int y)
    {
        if (grid.getBuilding(x, y) == 2)
        {
            //Replace return 1 with return bonus
            return 1;
        }
        return 0;
    }
    public int production()
    {
        return eps + buff;
    }
    public SortedSet<int> getValidFoundation()
    {
        return placeable;
    }
}

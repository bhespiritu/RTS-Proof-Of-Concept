﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Producer : MonoBehaviour
{

    int bldInt = 3;
    BuildingPlacer cubePlacer;
    GridR grid;
    private bool isPlaced = false;
    private int productionAmount;
    // Start is called before the first frame update
    void Start()
    {
        cubePlacer = FindObjectOfType<BuildingPlacer>();
        grid = cubePlacer.GetComponent<GridR>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            produceMass();
        }   
    }


    public void place()
    {
        isPlaced = true;
    }
    private void produceMass()
    {
        //Will use production amount to produce
        Debug.Log("Producing: " + productionAmount + checkBuff());
    }

    private int checkBuff()
    {
        //reset the buff in case nearby building have died
        int newBuff = 0;
        newBuff += checkBonus((int)this.transform.position.x - (int)grid.getSpacing(), (int)this.transform.position.z);
        newBuff += checkBonus((int)this.transform.position.x - (int)grid.getSpacing(), (int)this.transform.position.z - (int)grid.getSpacing());
        newBuff += checkBonus((int)this.transform.position.x - (int)grid.getSpacing(), (int)this.transform.position.z + (int)grid.getSpacing());
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
            //For now pylons will increase energy and mass
            return 1;
        }
        return 0;
    }
}

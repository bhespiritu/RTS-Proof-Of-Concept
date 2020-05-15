using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pylon : MonoBehaviour
{
    private int buffGiven = 1;
    BuildingPlacer cubePlacer;
    GridR grid;

    // Start is called before the first frame update
    void Start()
    {
        cubePlacer = FindObjectOfType<BuildingPlacer>();
        grid = cubePlacer.GetComponent<GridR>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void place()
    {
        // use for if we need to do anything when pylons are placed.
    }
    public int getBuff()
    {
        return buffGiven;
    }
}

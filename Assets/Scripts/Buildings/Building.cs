using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buildingPrefab;
    public GameObject buildingPrefab1;
    public GameObject buildingPrefab2;
    public GameObject buildingPrefab3;


    public int bldInt = 1;
    private void Start()
    {
       buildingPrefab = buildingPrefab1;
    }

    private void Update()
    {

    }

    public int getBldType()
    {
        return bldInt;
    }

    public GameObject getBuildingPrefab()
    {
        return buildingPrefab;
    }

    public GameObject switchBuilding(int x)
    {
        switch (x)
        {
            case 1:  buildingPrefab = buildingPrefab1;
                bldInt = 1;
                break;
            case 2:
                bldInt = 2;
                buildingPrefab = buildingPrefab2;
                break;
            case 3:
                bldInt = 3;
                buildingPrefab = buildingPrefab3;
                break;
        }
        return buildingPrefab;
    }

    public void place(GameObject placement)
    {
        switch (bldInt)
        {
            case 1:
                placement.GetComponent<EnergyProducer>().place();
                break;
            case 2:
                placement.GetComponent<Pylon>().place();
                break;
            case 3:
                bldInt = 3;
                buildingPrefab = buildingPrefab3;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Author: Roger Clanton
 * 
 * Controls the list of building that are placeable, used by BuildingPlacer.cs
 * Has to be updated everytime a new building type is added.
 * 
 * TODO:
 * Integrate with the UI
 */
public class BuildingController : MonoBehaviour
{
    //Todo figure out what each buildings ID is
    //Mass producer is 3
    //Energy producer is 4
    //Factory is 5
    //Pylon is 6
    public int bldInt;
    private Player player;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.TryGetComponent<MassProducer>(out MassProducer m))
        {
            bldInt = m.GetBldInt();
        }

        if (gameObject.TryGetComponent<EnergyProducer>(out EnergyProducer e))
        {
            bldInt = e.GetBldInt();
        }


        if (gameObject.TryGetComponent<Factory>(out Factory f))
        {
            bldInt = f.GetBldInt();
        }

        if (gameObject.TryGetComponent<Pylon>(out Pylon p))
        {
            bldInt = p.GetBldInt();
        }        
    }

    public int getBldType()
    {
        return bldInt;
    }

    public GameObject getBuildingPrefab()
    {
        switch (bldInt)
        {
            case (3):
                return gameObject.GetComponent<MassProducer>().getBuildingPrefab();
            case (4):
                return gameObject.GetComponent<EnergyProducer>().getBuildingPrefab();
            case (5):
                return gameObject.GetComponent<Factory>().getBuildingPrefab();
            case (6):
                return gameObject.GetComponent<Pylon>().getBuildingPrefab();
        }
        return null;
    }


    //Calls the placement function for each building type
    public void Place()
    {
        switch (bldInt)
        {
            case 3:
                gameObject.GetComponent<MassProducer>().place();
                break;
            case 4:
                gameObject.GetComponent<EnergyProducer>().place(player);
                break;
            case 5:
                gameObject.GetComponent<Factory>().place(player);
                break;
            case 6:
                gameObject.GetComponent<Pylon>().place();
                break;
            
        }
    }

    public void givePlayer(Player p)
    {
        player = p;
    }
}

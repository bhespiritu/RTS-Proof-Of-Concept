using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Roger Clanton
 * 
 * A class used to pass requests from a variety of building types to the player
 */

public class Requester : MonoBehaviour
{

    private int energy;
    
    public void setEnergy(int e)
    {
        energy = e;
    }

    public int getEnergy()
    {
        return energy;
    }

    // e is a percentage of energy requested
    public void giveEnergy(float e)
    {
        if (gameObject.TryGetComponent<EnergyProducer>(out EnergyProducer energy))
        {
            return;
        }
        if (gameObject.TryGetComponent<Pylon>(out Pylon pylon))
        {
            return;
        }
        if (gameObject.TryGetComponent<Producer>(out Producer producer))
        {
            return;
        }
        if (gameObject.TryGetComponent<Factory>(out Factory factory))
        {
            factory.work(e);
            return;
        }
    }
}

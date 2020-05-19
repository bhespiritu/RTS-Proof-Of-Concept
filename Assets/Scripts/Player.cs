using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int totalMass;
    public int totalEnergy = 0;
    public int totalUnits;
    public int maxEnergy = 5000;
    public int maxMass;
    public ulong score;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spendMass(int amount)
    {
        totalMass -= amount;
    }

    public void spendEnergy(int amount)
    {
        totalEnergy -= amount;
    }

    public int getMass()
    {
        return totalMass;
    }

    public int getEnergy()
    {
        return totalEnergy;
    }

    //Recieve input of resources from producers
    public void recieveMass(int mass)
    {
        totalMass += mass;
    }

    public void recieveEnergy(int energy)
    {
        if (totalEnergy + energy >= maxEnergy)
        {
            totalEnergy = maxEnergy;
        }
        else
        {
            totalEnergy += energy;
        }
    }
}

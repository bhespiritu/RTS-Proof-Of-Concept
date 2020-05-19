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
    private int energyRequested = 0;

    private List<Requester> requests;
    

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

    public void spendEnergy()
    {
        int curE = totalEnergy;
        int curReq = energyRequested;
        if(curReq <= curE)
        {
            totalEnergy -= curReq;
            distributeEnergy(1);
        }
        else
        {
            distributeEnergy(curE / curReq);
            totalEnergy -= curE;
        }


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

    public void energyRequest(Requester r, int e)
    {
        requests.Add(r);
        energyRequested += e;
    }

    //e is a percent 0-1
    private void distributeEnergy(float e)
    {
        foreach(Requester r in requests)
        {
            r.giveEnergy(e);
        }
    }
}

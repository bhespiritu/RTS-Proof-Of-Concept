using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int totalMass;
    public int totalEnergy = 0;
    public int totalUnits;
    public int maxEnergy = 200000000;
    public int maxMass;
    public ulong score;
    private int energyRequested = 0;

    private List<Requester> requests;
    

    // Start is called before the first frame update
    void Awake()
    {
        requests = new List<Requester>();
        Debug.Log("Length is: " + requests.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (energyRequested != 0)
        {
            spendEnergy();
        }
    }

    public void spendMass(int amount)
    {
        totalMass -= amount;
    }

    public void spendEnergy()
    {
        Debug.Log("Spending Energy");
        int curE = totalEnergy;
        int curReq = energyRequested;
        if(curReq <= curE)
        {
            totalEnergy -= curReq;
            energyRequested -= curReq;
            distributeEnergy(1);
        }
        else
        {
            distributeEnergy(curE / curReq);
            totalEnergy -= curE;
            energyRequested -= curReq;
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
        Debug.Log("Requested");
        requests.Add(r);
        energyRequested += e;
    }

    //e is a percent 0-1
    private void distributeEnergy(float e)
    {
        List<Requester> tempReq = requests;
        Debug.Log("Count is: " + requests.Count);
        if (tempReq.Count != 0)
        {
            foreach (Requester r in tempReq)
            {
                r.giveEnergy(e);
                requests.Remove(r);
            }
        }
    }
}

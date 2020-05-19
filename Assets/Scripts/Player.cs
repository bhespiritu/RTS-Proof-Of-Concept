using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("energy requested is: " + energyRequested);
        Debug.Log("Length of requests is: " + requests.Count);
        if (energyRequested != 0)
        {
            spendEnergy();
        }
        //Conditional to try to slap a fix on an error
        //Basically ensures that if energyRequested is 0 there shouldn't be any requests. This should just be true, but it isn't.
        if(energyRequested == 0)
        {
            requests.Clear();
        }
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
        if (!requests.Contains(r))
        {
            requests.Add(r);
            Debug.Log("Requesting more energy: " + e);
            energyRequested += e;
        }
    }

    //e is a percent 0-1
    private void distributeEnergy(float e)
    {
        List<Requester> tempReq = requests;
        Debug.Log("length of tempReq is: " + tempReq.Count);
        if (tempReq.Count != 0)
        {
            foreach (Requester r in tempReq)
            {
                r.giveEnergy(e);
                requests.Remove(r);
                Debug.Log("Length of requests after removal is: " + requests.Count);

            }
        }
    }
}

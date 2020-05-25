using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

/* Author: Max Wizneski and Roger Clanton
 * 
 * Manages player resources
 * 
 * TODO:
 */

public class Player : MonoBehaviour
{
    public int totalMass;
    public int totalEnergy = 0;
    public int totalUnits;
    public int maxEnergy = 200000000;
    public int maxMass;
    public ulong score;

    private List<Requester> requests;
    

    // Start is called before the first frame update
    void Awake()
    {
        requests = new List<Requester>();
        RoundTimeManager.OnRoundTick += onTick;
    }

    // Update is called once per frame
    void onTick()
    {
        if (requests.Count != 0)
        {
            spendEnergy();
        }
    }

    public int getMaxEnergy()
    {
        return maxEnergy;
    }

    public void spendMass(int amount)
    {
        totalMass -= amount;
    }

    public void spendEnergy()
    {
        List<Requester> r = buffer(requests);
        int curE = totalEnergy;
        int curReq = 0;
        int c = r.Count;
        if (c <= 0)
        {
            return;
        }

        for (int i = 0; i < c; i++)
        {
            curReq += r[i].getEnergy();
        }

        if(curReq <= curE)
        {
            totalEnergy -= curReq;
            distributeEnergy(1,r);
        }
        else
        {
            distributeEnergy(curE / curReq,r);
            totalEnergy -= curE;
        }
        Debug.Log("curReq is: " + curReq);
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
            r.setEnergy(e);
            requests.Add(r);
            Debug.Log("Requesting more energy: " + e);
        }
    }

    //e is a percent 0-1
    private void distributeEnergy(float e, List<Requester> r)
    {
        Debug.Log("e is: " + e);
        if (r.Count != 0)
        {
            foreach (Requester req in r)
            {
                req.giveEnergy(e);
            }
        }
    }


    private List<Requester> buffer(List<Requester> r)
    {
        int c = r.Count;
        List<Requester> newR = new List<Requester>();
        if (c != 0)
        {
            for (int i = 0; i < c; i++)
            {
                newR.Add(r[i]);
            }
            r.RemoveRange(0, c);
        }
        return newR;
    }

}

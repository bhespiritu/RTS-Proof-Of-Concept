using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int totalMass;
    public int totalEnergy;
    public int totalUnits;
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
}

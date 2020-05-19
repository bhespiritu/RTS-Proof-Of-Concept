using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requester : MonoBehaviour
{
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

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

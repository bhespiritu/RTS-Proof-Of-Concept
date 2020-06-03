using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSetVelocity : MonoBehaviour
{
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 v1 = new Vector3(3, 0, 0);
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = v1;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

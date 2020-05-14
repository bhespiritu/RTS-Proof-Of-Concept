using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveVelocity : MonoBehaviour
{
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
        if (Input.GetMouseButtonDown(1))
        {
            Rigidbody r = GetComponent<Rigidbody>();
            r.velocity = v1;
        }
    }
}

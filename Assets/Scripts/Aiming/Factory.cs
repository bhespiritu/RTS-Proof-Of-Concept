using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public GameObject unit1;
    public Player controller;
    public int health;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey("p")) && (controller.getMass() > 0))
        {
            i += 10;
            controller.spendMass(100);
            Debug.Log(controller.totalMass);
            GameObject instance = Instantiate(unit1, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.transform.position = new Vector3(55 + i, 55 + i, 55 + i);
        }
    }
}

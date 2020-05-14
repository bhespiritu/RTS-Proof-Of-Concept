using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapons : MonoBehaviour
{
    // Fields
    public GameObject target;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 trajectory(Vector3 v1, Vector3 v2)
    {
        float projVelo = 20.0f;
        Vector3 diff = v2 - v1;
        diff = diff.normalized * projVelo;
        return diff;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = transform.position;
        Vector3 v2 = target.transform.position;
        Vector3 v4 = trajectory(v1, v2);
        if (Input.GetMouseButtonDown(1))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = v4;
        }
    }
}

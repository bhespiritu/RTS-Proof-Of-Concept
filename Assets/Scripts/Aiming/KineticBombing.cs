using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticBombing : MonoBehaviour
{
    public GameObject target;
    public Rigidbody self;
    public Rigidbody targetBody;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {

    }

    public float dropDist(Vector3 v1, Vector3 v2, Vector3 ownVelocity, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        Vector3 v3 = v2 + targetVelocity - v1;
        float h = v3.y;
        v3.y = 0;
        ownVelocity.y = 0;
        float dist = ownVelocity.magnitude * Mathf.Sqrt(-2 * h / grav);
        if (dist <= v3.magnitude)
        {
            return dist;
        }
        else
        {
            return -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = transform.position;
        Vector3 v2 = target.transform.position;
        Vector3 velo1 = self.velocity;
        Vector3 velo2 = targetBody.velocity;
        Vector3 v3 = v2 + velo2 - v1;
        v3.y = 0;
        Debug.Log("drop dist: " + (int) dropDist(v1, v2, velo1, velo2));
        Debug.Log("dist: " + (int) v3.magnitude);
        if ((int) dropDist(v1, v2, velo1, velo2) == (int) v3.magnitude) 
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = self.velocity;
        }
    }
}

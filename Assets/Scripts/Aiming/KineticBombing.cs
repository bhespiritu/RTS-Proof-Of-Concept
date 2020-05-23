using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticBombing : MonoBehaviour
{
    public GameObject target;
    public Rigidbody self;
    public Rigidbody targetBody;
    public GameObject bullet;
    public float time;
    public float startTime;
    // Start is called before the first frame update
    void Start()
    {
        time = -1;
    }

    public float dropTime(Vector3 v1, Vector3 v2, Vector3 ownVelocity, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        Vector3 v3 = v2 + targetVelocity - v1;
        float h = v3.y;
        v3.y = 0;
        ownVelocity.y = 0;
        if ((ownVelocity.magnitude * Mathf.Sqrt(2*h/grav) > v3.magnitude))
        {
            return (v3.magnitude / ownVelocity.magnitude) - (ownVelocity.magnitude * Mathf.Sqrt(2 * h / grav));
        }
        else
        {
            return -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        Vector3 v1 = transform.position;
        Vector3 v2 = target.transform.position;
        Vector3 velo1 = self.velocity;
        Vector3 velo2 = targetBody.velocity;
        if (Input.GetMouseButtonDown(0))
        {
            startTime = Time.time;
            time = dropTime(v1, v2, velo1, velo2);
        }
        if ((time > -1) && (time == (currentTime - startTime))) 
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = self.velocity;
        }
    }
}

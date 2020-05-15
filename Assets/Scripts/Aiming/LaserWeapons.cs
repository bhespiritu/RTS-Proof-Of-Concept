using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapons : MonoBehaviour
{
    // Fields
    public GameObject target;
    public GameObject bullet;
    public GameObject bullet1;
    public GameObject mobileTarget;
    public Rigidbody mobileTargetBody;
    // Start is called before the first frame update
    void Start()
    {
        mobileTargetBody = mobileTarget.GetComponent<Rigidbody>();
    }

    Vector3 trajectory(Vector3 v1, Vector3 v2, Vector3 targetVelocity)
    {
        float projVelo = 20.0f;
        Vector3 diff = v2 + (targetVelocity * ((v2-v1).magnitude / projVelo)) - v1;
        diff = diff.normalized * projVelo;
        return diff;
    }

    Vector3 trajectory(Vector3 v1, Vector3 v2)
    {
        return trajectory(v1, v2, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = transform.position;
        Vector3 v2 = target.transform.position;
        Vector3 v3 = mobileTarget.transform.position;
        Vector3 v4 = trajectory(v1, v2);
        Vector3 v5 = trajectory(v1, v3, mobileTargetBody.velocity);
        Debug.DrawRay(transform.position, 5 * v4.normalized, Color.gray);
        //Debug.DrawRay(transform.position, 5 * v5.normalized, Color.gray);
        if (Input.GetMouseButtonDown(2))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = v4;
            GameObject instance1 = Instantiate(bullet1, transform.position, Quaternion.identity);
            Rigidbody r1 = instance1.GetComponent<Rigidbody>();
            r1.velocity = v4;
        }
        if (Input.GetMouseButtonDown(2))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = v5;
        }
    }
}

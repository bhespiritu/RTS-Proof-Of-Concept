using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticWeapons : MonoBehaviour
{
    // Fields
    public GameObject staticTarget;
    public GameObject bullet;
    public GameObject mobileTarget;
    public Rigidbody mobileTargetBody;
    // Start is called before the first frame update
    void Start()
    {
        mobileTargetBody = mobileTarget.GetComponent<Rigidbody>();
    }

    //Helper method to determine the angle of fire
    float angleOfFire(float heightdiff, float distance, float aggregateVal)
    {
        return (Mathf.Acos(((2 * aggregateVal) - heightdiff) / Mathf.Sqrt(Mathf.Pow(heightdiff, 2) + Mathf.Pow(distance, 2))) + Mathf.Atan2(distance, heightdiff)) / 2;
    }

    //Calculates the maximum path the projectile could take
    Vector3 maximumTrajectory(Vector3 v1, Vector3 v2, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        float projVelo = 100.0f;
        Vector3 diff = v2 - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = angleOfFire(h, d, a);
        float time = d / (projVelo * Mathf.Cos(theta));
        Vector3 v3 = v2 + (targetVelocity * time);
        Vector3 diff1 = v3 - v1;
        float h1 = -diff1.y;
        diff1.y = 0;
        float d1 = diff1.magnitude;
        float a1 = (grav * Mathf.Pow(d1, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta1 = angleOfFire(h1, d1, a1);
        diff1 = diff1.normalized;
        diff1 = diff1 * projVelo * Mathf.Cos(theta1);
        diff1.y = projVelo * Mathf.Sin(theta1);
        return diff1;
    }

    Vector3 maximumTrajectory(Vector3 v1, Vector3 v2)
    {
        return maximumTrajectory(v1, v2, Vector3.zero);
    }

    //Leaving the static solution in, in case it is relevant
   /* Vector3 maximumStaticTrajectory(Vector3 v1, Vector3 v2, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        float projVelo = 20.0f;
        Vector3 diff = v2 + targetVelocity - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = angleOfFire(h, d, a);
        diff = diff.normalized;
        diff = diff * projVelo * Mathf.Cos(theta);
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    Vector3 maximumStaticTrajectory(Vector3 v1, Vector3 v2)
    {
        return maximumTrajectory(v1, v2, Vector3.zero);
    }*/

    //Calculates the minimum path of the projectile could take
    Vector3 minimumTrajectory(Vector3 v1, Vector3 v2, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        float projVelo = 20.0f;
        Vector3 diff = v2 + (targetVelocity * ((v2 - v1).magnitude / projVelo)) - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = angleOfFire(h, d, a);
        theta = (Mathf.PI / 2) - theta - Mathf.Sin(h / d);
        diff = diff.normalized;
        diff = diff * projVelo * Mathf.Cos(theta);
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    Vector3 minimumTrajectory(Vector3 v1, Vector3 v2)
    {
        return minimumTrajectory(v1, v2, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = transform.position;
        Vector3 v2 = staticTarget.transform.position;
        Vector3 vt = mobileTarget.transform.position;
        Vector3 v2_2 = mobileTarget.transform.position;
        Vector3 v2_2_2 = v2_2 + mobileTargetBody.velocity;
        Vector3 v3 = maximumTrajectory(v1, v2);
        Vector3 v4 = minimumTrajectory(v1, v2);
        Vector3 v5 = minimumTrajectory(v1, vt, mobileTargetBody.velocity);
        Vector3 v6 = maximumTrajectory(v1, vt, mobileTargetBody.velocity);
        Vector3 diff = v2 - v1;
        Vector3 diff2 = v2_2 - v1;
        Vector3 diff3 = v2_2_2 - v1;
        //Debug.DrawRay(transform.position, diff, Color.magenta);
        Debug.DrawRay(transform.position, diff2, Color.cyan);
        Debug.DrawRay(transform.position, diff3, Color.blue);
        // Debug.DrawRay(transform.position, 5 * v4.normalized, Color.green);
        // Debug.DrawRay(transform.position, 5 * v3.normalized, Color.red);
        //Debug.DrawRay(transform.position, 5 * v5.normalized, Color.yellow);
        // Debug.DrawRay(transform.position, 5 * v6.normalized, Color.black);
        if (Input.GetMouseButtonDown(0))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = v4;
        }
        if (Input.GetMouseButtonDown(0))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = v3;
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = v5;
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            r.velocity = v6;
        }
    }
}

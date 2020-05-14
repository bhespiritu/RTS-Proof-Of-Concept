using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticWeapons : MonoBehaviour
{
    // Fields
    public GameObject target;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
    }

    Vector3 maximumMobileTrajectory(Vector3 v1, Vector3 v2, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        float projVelo = 20.0f;
        Vector3 diff = v2 - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = (Mathf.Acos(((2 * a) - h) / Mathf.Sqrt(Mathf.Pow(h, 2) + Mathf.Pow(d, 2))) + Mathf.Atan2(d, h)) / 2;
        diff = diff.normalized;
        diff = diff * projVelo * Mathf.Cos(theta);
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    Vector3 maximumStaticTrajectory(Vector3 v1, Vector3 v2)
    {
        float grav = 9.8f;
        float projVelo = 20.0f;
        Vector3 diff = v2 - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = (Mathf.Acos(((2 * a) - h) / Mathf.Sqrt(Mathf.Pow(h, 2) + Mathf.Pow(d, 2))) + Mathf.Atan2(d, h))/2;
        theta = (Mathf.PI / 2) - theta - Mathf.Sin(h/d);
        diff = diff.normalized;
        diff = diff * projVelo * Mathf.Cos(theta);
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    Vector3 minimumStaticTrajectory(Vector3 v1, Vector3 v2)
    {
        float grav = 9.8f;
        float projVelo = 20.0f;
        Vector3 diff = v2 - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = (Mathf.Acos(((2 * a) - h) / Mathf.Sqrt(Mathf.Pow(h, 2) + Mathf.Pow(d, 2))) + Mathf.Atan2(d, h)) / 2;
        //theta = (Mathf.PI / 2) - theta + Mathf.Atan2(d, h);
        diff = diff.normalized;
        diff = diff * projVelo * Mathf.Cos(theta);
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = transform.position;
        Vector3 v2 = target.transform.position;
        Vector3 v3 = maximumStaticTrajectory(v1, v2);
        Vector3 v4 = minimumStaticTrajectory(v1, v2);
        Vector3 diff = v2 - v1;
        Debug.DrawRay(transform.position, diff, Color.blue);
        Debug.DrawRay(transform.position, 5 * v4.normalized, Color.green);
        Debug.DrawRay(transform.position, 5 * v3.normalized, Color.red);
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
    }
}

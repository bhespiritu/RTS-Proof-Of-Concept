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

    bool straightLine(Vector3 v1, Vector3 v2)
    {
        return true;
    }

    Vector3 trajectory1(Vector3 v1, Vector3 v2)
    {
        float grav = 9.8f;
        float projVelo = 200.0f;
        Vector3 diff = v2 - v1;
        diff.y = 0;
        diff = diff.normalized;
        float val = (4 * Mathf.Sqrt(v1.y + (Mathf.Pow(projVelo, 2) / (2 * grav)) - v2.y));
        float theta = Mathf.Atan(1/val);
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    Vector3 trajectory2(Vector3 v1, Vector3 v2)
    {
        float grav = 9.8f;
        float projVelo = 20.0f;
        Vector3 diff = v2 - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = (Mathf.Acos(((2 * a) - h) / Mathf.Sqrt(Mathf.Pow(h, 2) + Mathf.Pow(d, 2))) + Mathf.Atan2(d, h))/2;
        Debug.Log(Mathf.Rad2Deg*theta);
        diff = diff.normalized;
        diff = diff * projVelo * Mathf.Cos(theta);
/*        if (diff.z > 0)
        {
            diff.z = -diff.z;
            Debug.Log("error");
        }*/
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = transform.position;
        Vector3 v2 = target.transform.position;
        Vector3 v4 = trajectory2(v1, v2);
        Vector3 diff = v2 - v1;
        Debug.DrawRay(transform.position, diff, Color.blue);
        //Debug.DrawRay(transform.position, 5 * v4.normalized, Color.green);
        Debug.DrawRay(transform.position, v4, Color.green);
        if (Input.GetMouseButtonDown(0))
        {
            GameObject instance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody r = instance.GetComponent<Rigidbody>();
            Debug.Log(v4);
            r.velocity = v4;
        }
    }
}

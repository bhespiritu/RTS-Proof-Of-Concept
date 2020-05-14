using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadingAgent : MonoBehaviour
{

    public GameObject debugMarker;

    public float turningSpeed = 180; //in degrees per second
    public float moveSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetDirection = debugMarker.transform.position - transform.position;
        targetDirection.y = 0;
        float currentAngle = Vector3.SignedAngle(transform.forward, targetDirection,Vector3.up);
        if (Mathf.Abs(currentAngle) >= turningSpeed * Time.fixedDeltaTime)
        {
            float sign = Mathf.Sign(currentAngle);
            Vector3 rot = transform.localEulerAngles;
            rot.y += sign * turningSpeed * Time.fixedDeltaTime;
            transform.localEulerAngles = rot;
        } else
        {
            transform.forward = targetDirection;
        }

        if(targetDirection.sqrMagnitude > 1)
            transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime * (1-(Mathf.Abs(currentAngle)/180));
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}

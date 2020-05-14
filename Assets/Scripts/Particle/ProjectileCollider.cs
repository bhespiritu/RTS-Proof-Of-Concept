using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2) { 
            Debug.Log("COLLISION DETECTED");
            Destroy(gameObject);
            //TODO: Add any interactions with the collisions here
        }
    }
}

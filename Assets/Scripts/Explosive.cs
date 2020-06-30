using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{

    public int damage = 0;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject + " " + collision.gameObject);
        
        if(collision.gameObject.TryGetComponent<Unit>(out Unit unit))
        {
            unit.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}

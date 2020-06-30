using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    public float lifetime = 2;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

}

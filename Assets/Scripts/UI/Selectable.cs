using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public static List<Selectable> allSelectable = new List<Selectable>();

    public GameObject selectionObject;

    // Start is called before the first frame update
    void Awake()
    {
        allSelectable.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

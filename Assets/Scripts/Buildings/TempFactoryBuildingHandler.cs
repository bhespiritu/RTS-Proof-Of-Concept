using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TempFactoryBuildingHandler : MonoBehaviour
{
    [SerializeField]
    public GameObject gui;
    public GameObject panel;


    // Start is called before the first frame update
    void Start()
    {
        GameObject square = Instantiate(panel,gui.transform);
        square.transform.parent = gui.transform;
        square.SetActive(false);         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

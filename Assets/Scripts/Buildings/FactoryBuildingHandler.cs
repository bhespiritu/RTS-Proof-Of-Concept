using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryBuildingHandler : MonoBehaviour
{
    [SerializeField]
    public GameObject gui;
    public GameObject panel;
    public GameObject button;

    private List<GameObject> uiElements;

    // Start is called before the first frame update
    void Start()
    {
        uiElements = new List<GameObject>();
        GameObject square = Instantiate(panel,gui.transform);
        square.transform.SetParent(gui.transform);
        square.transform.localScale = new Vector3(.2f, .2f, .2f);
        square.transform.localPosition = gui.transform.localPosition + new Vector3(-940, -1000);
        if(square.TryGetComponent<Image>(out Image i)){
            i.color = Color.blue;
        }
        //square.SetActive(false);
        uiElements.Add(square);
    }

    public List<GameObject> GetUIElements()
    {
        return uiElements;
    }

  
}

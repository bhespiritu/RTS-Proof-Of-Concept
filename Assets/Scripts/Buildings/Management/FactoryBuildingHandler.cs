using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryBuildingHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject gui;
    public GameObject panel;
    public GameObject button;

    private List<GameObject> uiElements;

    // Start is called before the first frame update
    void Start()
    {
        uiElements = new List<GameObject>();
    }
    //Test method to make a blue square in the gui
    public void CreateSquare()
    {
        GameObject square = Instantiate(panel, gui.transform);
        square.transform.SetParent(gui.transform);
        square.transform.localScale = new Vector3(.2f, .2f, .2f);
        square.transform.localPosition = gui.transform.localPosition + new Vector3(-940, -1000);
        if (square.TryGetComponent<Image>(out Image i))
        {
            i.color = Color.blue;
        }
        square.SetActive(false);
        uiElements.Add(square);
    }

    public List<GameObject> GetUIElements()
    {
        return uiElements;
    }

    public void GiveGui(GameObject g)
    {
        gui = g;
        CreateSquare(); //The elements don't need to be created here if no reference to the gui is used in their creation
    }

    //Activate elements should be called on select and deactivate on deselect. Only if it's the only thing selected however.
    public void ActivateElements()
    {
        foreach(GameObject g in uiElements)
        {
            g.SetActive(true);
        }
    }

    public void DeactivateElements()
    {
        foreach (GameObject g in uiElements)
        {
            g.SetActive(false);
        }
    }

}

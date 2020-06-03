using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Holds the list of temporary UI elements for the selected building
 */

public class UIBuildingManager : MonoBehaviour
{
    private List<GameObject> uiElements;

    private void Awake()
    {
        uiElements = new List<GameObject>();
    }

    public void deactivateElements()
    {
        foreach(GameObject g in uiElements)
        {
            g.SetActive(false);
        }
    }

    public void activateElements()
    {
        foreach (GameObject g in uiElements)
        {
            g.SetActive(true);
        }
    }

    public void clearElementsList()
    {
        deactivateElements();
        uiElements.Clear();
    }

    public void changeElements(List<GameObject> elements)
    {
        clearElementsList();
        uiElements = elements;
        activateElements();
    }

}

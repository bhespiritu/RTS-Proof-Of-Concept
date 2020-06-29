using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryBuildingHandler : MonoBehaviour
{
    private List<GameObject> uiElements;

    [SerializeField]
    private GameObject gui;
    private GameObject panel;
    private GameObject buttonPrefab;
    private Selectable selectable;

    

    // Start is called before the first frame update
    void Awake()
    {
        uiElements = new List<GameObject>();
        selectable = gameObject.GetComponent<Selectable>();
        selectable.OnSelect += ActivateElements;
        selectable.OnDeselect += DeactivateElements;
    }

    private void CreateButton()
    {
        buttonPrefab = FindObjectOfType<BuildUIPrefabs>().GetButtonPrefab();
        GameObject button = Instantiate(buttonPrefab, gui.transform);
        button.transform.localScale = new Vector3(1f, 1f, 1f);
        button.transform.localPosition = gui.transform.localPosition + new Vector3(-940, -1000);
        uiElements.Add(button);
        button.SetActive(false);
    }
    //Test method to make a blue square in the gui
    private void CreateSquare()
    {
        panel = FindObjectOfType<BuildUIPrefabs>().GetPanelPrefab();
        GameObject square = Instantiate(panel, gui.transform);
        square.transform.SetParent(gui.transform);
        square.transform.localScale = new Vector3(.2f, .2f, .2f);
        square.transform.localPosition = gui.transform.localPosition + new Vector3(-940, -1000);
        if (square.TryGetComponent<Image>(out Image i))
        {
            i.color = Color.blue;
        }
        uiElements.Add(square);
        square.SetActive(false);
    }

    public List<GameObject> GetUIElements()
    {
        return uiElements;
    }

    public void GiveGui(GameObject g)
    {
        gui = g;
        CreateSquare(); //The elements don't need to be created here if no reference to the gui is used in their creation
        CreateButton();
        ActivateElements(); //Test line
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

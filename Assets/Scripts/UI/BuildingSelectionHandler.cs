using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingSelectionHandler : MonoBehaviour
{
    GameObject gui;
    public UIBuildingManager uiManager;

    private RaycastHit clickCast = new RaycastHit();
    public void OnPointerClick(PointerEventData eventData)
    {

        Ray clickWorldRay = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(clickWorldRay, out clickCast))
        {
            //If a building is clicked then call its methods to put up its UI elements
            if (clickCast.transform.gameObject.TryGetComponent<Building>(out Building bld))
            {
                //Have the GUI deactivate everything in its list of selected

                //Have the building activate all of its gui elements, and set them as the gui's selected
            }
        }
    }

    public void OnSelect(Building bld)
    {
        uiManager.changeElements(bld.getElements());
    }
}

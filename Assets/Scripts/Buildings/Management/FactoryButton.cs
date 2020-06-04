using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryButton : MonoBehaviour
{

    public GameObject buildingPrefab;

    public MouseHandler msHandler;
    public void OnClick()
    {
        //If the user was placing a factory, stop.
        Debug.Log("Cursor type int " + msHandler.GetCursorType());
        Debug.Log("factory type int " + buildingPrefab.GetComponent<BuildingController>().getBldType());
        Debug.Log(msHandler.GetMouseMode());
        if (msHandler.GetMouseMode() == MouseMode.Placement && msHandler.GetCursorType() == buildingPrefab.GetComponent<BuildingController>().getBldType())
        {
            msHandler.SwitchToSelect();
        }
        else
        {
            msHandler.SwitchToPlacement(buildingPrefab);

        }
    }
}

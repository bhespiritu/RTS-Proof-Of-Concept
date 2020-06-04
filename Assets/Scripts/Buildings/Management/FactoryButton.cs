using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryButton : MonoBehaviour
{

    public GameObject buildingPrefab;

    public MouseHandler msHandler;
    public void OnClick()
    {
        //If the user was placing a factory, stop   
        if (msHandler.GetMouseMode() == MouseMode.Placement && msHandler.GetCursorType() == 5)
        {
            msHandler.SwitchToSelect();
        }
        else
        {
            msHandler.SwitchToPlacement(buildingPrefab);

        }
    }
}

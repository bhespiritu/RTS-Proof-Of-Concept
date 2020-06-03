using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryButton : MonoBehaviour
{

    private GameObject buildingPrefab;

    public GameObject cursor;

    public Material placeable;
    public Material unplaceable;

    private MeshRenderer mesh;
    private MeshRenderer childMesh;
    private BuildingController building;
    public Player player;

    public MouseHandler msHandler;
    void OnClick()
    {
        msHandler.SwitchToPlacement(buildingPrefab.GetComponent<BuildingController>());
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * Author: Roger Clanton
 * 
 * Places buildings through the UI
 * 
 * TODO:
 * Add new building and placement types as we create them
 * Integrate with the UI
 */
public class BuildingPlacer : MonoBehaviour
{

    public GridR grid;
    public Player player;

    public Material placeable;
    public Material unplaceable;
    public Material notConstructed;
    public Material finishedConstructing;

    private GameObject buildingPrefab;

    public GameObject cursor;

    private MeshRenderer mesh;
    private MeshRenderer childMesh;

    public LayerMask placementLayerMask;

    public LayerMask buildingsLayerMask;

    private Building building;

    
    private void Awake() {

        //Find the prefab from building
        building = FindObjectOfType<Building>();
        building.givePlayer(player);
        buildingPrefab = building.getBuildingPrefab();
        //Find the grid, stores information about whether a space is placeable
        //Glowy construct of where the building will be placed
        cursor = Instantiate(buildingPrefab);
        //Layers and mesh for cursor
        mesh = cursor.GetComponent<MeshRenderer>();
        cursor.layer = 8;
        foreach (Transform child in cursor.transform)
        {
            child.gameObject.layer = 8;
            childMesh = child.GetComponent<MeshRenderer>();
            childMesh.material = placeable;
        }


    }

    private void Update() {
        //Shoot a ray to the mouse
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //set local so the cursor will keep updating
        Vector3 localUp = cursor.transform.up;
        Vector3 localForward = cursor.transform.forward;
        //If the cursor is above a placable terrain
        if (gameObject.activeSelf)
        {
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placementLayerMask))
            {

                //Snap to the grid
                cursor.transform.position = grid.getGridPoint(hitInfo.point);
                //New offset so it fits on the grid spot
                Vector3 pos = new Vector3(0f, 4.5f, 0f);
                cursor.transform.position += pos;
                localUp = hitInfo.normal;
                cursor.transform.position += hitInfo.normal * 0.5f;

                if (checkPosition())
                {
                    //Set the material of the cursor to the placeable color
                    mesh.material = placeable;
                    foreach (Transform child in cursor.transform)
                    {
                        child.gameObject.layer = 8;
                        childMesh = child.GetComponent<MeshRenderer>();
                        childMesh.material = placeable;
                    }
                }
                else
                {
                    mesh.material = unplaceable;
                    foreach (Transform child in cursor.transform)
                    {
                        child.gameObject.layer = 8;
                        childMesh = child.GetComponent<MeshRenderer>();
                        childMesh.material = unplaceable;
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buildingPrefab = building.switchBuilding(1);
                changeCursor();
                
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buildingPrefab = building.switchBuilding(2);
                changeCursor();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buildingPrefab = building.switchBuilding(3);
                changeCursor();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buildingPrefab = building.switchBuilding(4);
                changeCursor();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 temp = cursor.transform.forward;

                temp.x = -cursor.transform.forward.z;
                temp.z = cursor.transform.forward.x;
                localForward = temp;

            }

            cursor.transform.LookAt(cursor.transform.position + localForward, localUp);


            if (Input.GetMouseButtonDown(0))
            {

                if (checkPosition())
                {
                    PlaceBuildingNear();
                }
            }
        }

    }

    private void changeCursor()
    {
        Destroy(cursor);
        cursor = Instantiate(buildingPrefab);
        //Layers and mesh for cursor
        mesh = cursor.GetComponent<MeshRenderer>();
        cursor.layer = 8;
        foreach (Transform child in cursor.transform)
        {
            child.gameObject.layer = 8;
            childMesh = child.GetComponent<MeshRenderer>();
            childMesh.material = placeable;
        }
    }

    public GridR getGrid()
    {
        return grid;
    }
    private bool checkPosition()
    {
        //plus 5 comes from the offset
        int bldType = grid.getBuilding(cursor.transform.position.x, cursor.transform.position.z);
        SortedSet<int> validTypes = getValidFoundations();
        if (validTypes.Contains(bldType))
        {
            
            foreach (Transform child in cursor.transform)
            {
                if (!validTypes.Contains(grid.getBuilding(child.position.x, child.position.z)))
                {
                    return false;
                }
            }
        }
        else { return false; }
        return true;
    }

    private SortedSet<int> getValidFoundations()
    {
        if (cursor.TryGetComponent<EnergyProducer>(out EnergyProducer energy))
        {
            return energy.getValidFoundation();
        }
        if (cursor.TryGetComponent<Pylon>(out Pylon pylon))
        {
            return pylon.getValidFoundation();
        }
        if (cursor.TryGetComponent<Producer>(out Producer producer))
        {
            return producer.getValidFoundation();
        }
        if (cursor.TryGetComponent<Factory>(out Factory factory))
        {
            return factory.getValidFoundation();
        }
        return new SortedSet<int> { };
    }
    
    private void PlaceBuildingNear(){
        //Place the object based on the normal
        GameObject build = GameObject.Instantiate(buildingPrefab);
        
        build.transform.position = cursor.transform.position;
        build.transform.rotation = cursor.transform.rotation;

        //update the grid
        grid.updateBuilding(build.transform.position.x, build.transform.position.z, building.getBldType());
        foreach (Transform child in cursor.transform)
        {
            grid.updateBuilding(child.position.x, child.position.z, building.getBldType());
            building.place(build);
        }
    }

}

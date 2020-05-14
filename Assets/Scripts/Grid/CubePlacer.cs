using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class CubePlacer : MonoBehaviour
{

    private GridR grid;

    public Material placeable;
    public Material unplaceable;

    public GameObject buildingPrefab;

    public GameObject cursor;

    private MeshRenderer mesh;
    private MeshRenderer childMesh;

    public LayerMask placementLayerMask;

    public LayerMask buildingsLayerMask;

    private void Awake() {

        //Find the grid, stores information about whether a space is placeable
        grid = FindObjectOfType<GridR>();
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

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placementLayerMask) && checkPosition())
                {
                    PlaceCubeNear();
                }
            }
        }

    }

    private bool checkPosition()
    {
        int bldType = grid.getBuilding(cursor.transform.position.x, cursor.transform.position.z);
        if (bldType != 0 && bldType != 3)
        {
            foreach (Transform child in cursor.transform)
            {
                if (grid.getBuilding(child.position.x, child.position.z) != bldType)
                {
                    return false;
                }
            }
        }
        else { return false; }
        return true;
    }
    
    private void PlaceCubeNear(){
        //Place the object based on the normal
        GameObject cube = GameObject.Instantiate(buildingPrefab);
        cube.transform.position = cursor.transform.position;
        cube.transform.rotation = cursor.transform.rotation;

        //update the grid
        grid.updateBuilding(cube.transform.position.x, cube.transform.position.z, 3);
        foreach (Transform child in cursor.transform)
        {
            grid.updateBuilding(child.position.x, child.position.z, 3);
        }
    }
}

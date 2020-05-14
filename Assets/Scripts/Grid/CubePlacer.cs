using System.Collections;
using System.Collections.Generic;
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

    private void Awake(){
      
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

    private void Update(){

        //Shoot a ray to the mouse
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //set local so the cursor will keep updating
        Vector3 localUp = cursor.transform.up;
        Vector3 localForward = cursor.transform.forward;

        //If the cursor is above a placable terrain
        if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity,placementLayerMask))
        {
            
            //Snap to the grid
            cursor.transform.position = grid.getGridPoint(hitInfo.point);
            localUp = hitInfo.normal;
            cursor.transform.position += hitInfo.normal * 0.5f;

            //Set the material of the cursor to the placeable color
            mesh.material = placeable;
            foreach (Transform child in cursor.transform)
            {
                child.gameObject.layer = 8;
                childMesh = child.GetComponent<MeshRenderer>();
                childMesh.material = placeable;
            }
        }

        if (Input.GetKeyDown(KeyCode.A)){
            Vector3 temp = cursor.transform.forward;

            temp.x = -cursor.transform.forward.z;
            temp.z = cursor.transform.forward.x;
            localForward = temp;

        }

        cursor.transform.LookAt(cursor.transform.position + localForward, localUp);

        Ray downRay = new Ray(cursor.transform.position, Vector3.down);
        Debug.DrawRay(cursor.transform.position, Vector3.down);

        if (Physics.Raycast(downRay, out hitInfo, Mathf.Infinity, buildingsLayerMask))
        {
            mesh.material = unplaceable;
            foreach (Transform child in cursor.transform)
            {
                child.gameObject.layer = 8;
                childMesh = child.GetComponent<MeshRenderer>();
                childMesh.material = unplaceable;
            }
        }

            if (Input.GetMouseButtonDown(0)){
            
            if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placementLayerMask)){
                PlaceCubeNear();
            }
        }
        

    }

    
    private void PlaceCubeNear(){
        
        GameObject cube = GameObject.Instantiate(buildingPrefab);
        cube.transform.position = cursor.transform.position;
        cube.transform.rotation = cursor.transform.rotation;
    }
}

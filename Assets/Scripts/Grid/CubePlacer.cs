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

    private void Awake(){
      
        grid = FindObjectOfType<GridR>();
        cursor = Instantiate(buildingPrefab);
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

        

        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity,placementLayerMask))
        {
            cursor.transform.position = hitInfo.point;
            mesh.material = placeable;
            foreach (Transform child in cursor.transform)
            {
                child.gameObject.layer = 8;
                childMesh = child.GetComponent<MeshRenderer>();
                childMesh.material = placeable;
            }
        }

        if (Input.GetMouseButtonDown(0)){
            
            if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placementLayerMask)){
                PlaceCubeNear(hitInfo.point);
            }
        }
        

    }

    
    private void PlaceCubeNear(Vector3 clickPoint){
        var finalPosition = grid.getGridPoint(clickPoint);
        RaycastHit hitInfo;
        GameObject cube = GameObject.Instantiate(buildingPrefab);
        cube.transform.position = finalPosition;
        Physics.Raycast(cube.transform.position + 10*Vector3.up, Vector3.down, out hitInfo);
        cube.transform.up = hitInfo.normal;
        cube.transform.position +=  hitInfo.normal * 0.5f; 
    }
}

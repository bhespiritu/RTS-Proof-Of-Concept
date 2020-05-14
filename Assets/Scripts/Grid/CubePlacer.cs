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

        Vector3 localUp = cursor.transform.up;
        Vector3 localForward = cursor.transform.forward;

        if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity,placementLayerMask))
        {
            
            cursor.transform.position = grid.getGridPoint(hitInfo.point);
            localUp = hitInfo.normal;
            cursor.transform.position += hitInfo.normal * 0.5f;
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

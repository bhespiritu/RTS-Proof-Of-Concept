using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridR : MonoBehaviour
{


    // # of units between grid points
    [SerializeField]
    private float spacing = 1f;

    public Vector3 getGridPoint(Vector3 position){
        //Subtract off its coordinates before multiplication
        position -= transform.position;

        int xTranformations = Mathf.RoundToInt(position.x / spacing);
        int yTranformations = Mathf.RoundToInt(position.y / spacing);
        int zTranformations = Mathf.RoundToInt(position.z / spacing);

        Vector3 newVector = new Vector3( (float)xTranformations * spacing, 
            (float)yTranformations * spacing, (float)zTranformations * spacing);  

        newVector += transform.position;

        return newVector;
    
     }
   
   //Test to draw spheres on the points of the grid
   private void OnDrawGizmos(){
       Gizmos.color = Color.yellow;
       for(float x = 0; x<50; x+= spacing){
           for(float y = 0; y<50; y+= spacing){
               var dot = getGridPoint(new Vector3(x,0f,y));
               Gizmos.DrawSphere(dot,.1f);
           }
       }
   }
}

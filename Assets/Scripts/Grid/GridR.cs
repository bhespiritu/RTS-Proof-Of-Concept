using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * Author: Roger Clanton
 * 
 * Creates a grid that keeps track of where buildings can be placed
 * 
 * TODO:
 * Add new building and placement types as we create them
 * Implement the ability for terrain to be damaged?
 */

public class GridR : MonoBehaviour
{

    public LayerMask placementLayerMask;

    public LayerMask buildingsLayerMask;

    // # of units between grid points
    [SerializeField]
    private float spacing = 10f;

    private int mapSizeX = 1000;
    private int mapSizeY = 1000;

    private int[,] bldSpots = new int[1000,1000];

    private void Awake()
    {
        createMap();
    }
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

   public int getBuilding(float x, float y)
    {
        if (x >= mapSizeX || x < 0 || y >= mapSizeY || y < 0)
        {
            return 0;
        }
        return bldSpots[(int)x, (int)y];
    }


    public void updateBuilding(float x, float y, int bld)
    {
        if (x >= mapSizeX || x < 0 || y >= mapSizeY || y < 0)
        {
            return;
        }
        bldSpots[(int)x, (int)y] = bld;
    }


    //Test to draw spheres on the points of the grid
    private void OnDrawGizmos(){
       Gizmos.color = Color.yellow;
       for(float x = 0; x<1000; x+= spacing){
           for(float y = 0; y<1000; y+= spacing){
               var dot = getGridPoint(new Vector3(x,0f,y));
               dot.y += 50;
               Gizmos.DrawSphere(dot,1f);
           }
       }
    }

    private void createMap()
    {
        //Path file for map
        string path = Application.dataPath + "/Scripts/Grid/map.txt";
        if (!File.Exists(path))
        {
            Debug.Log("Making Map");
            // Map width then map height
            for (int i = 0; i < 1000; i += (int)spacing)
            {
                for (int j = 0; j < 1000; j += (int)spacing)
                {
                    determinePlacability(i, j);
                }
            }

            string data = "";
            for(int i = 0; i < 1000; i += (int)spacing)
            {
                string line = "";
                for (int j = 0; j < 1000; j += (int)spacing)
                {
                    line = line + " " + bldSpots[i, j];
                }
                data = data + line + "\n";
            }
            File.WriteAllText(path, data);
        }
        else
        {
            FileInfo theSourceFile = new FileInfo(path);
            StreamReader reader = theSourceFile.OpenText();

            string text;
            int i = 0;
            text = reader.ReadLine();
            while (text != null)
            {
                //Console.WriteLine(text);

                string[] info = text.Split(' ');
                for (int j = 0; j <100; j++)
                {
                    int number;
                    Int32.TryParse(info[j+1], out number);
                    //Debug.Log("j is: " +j + " j*spacing is: " + (j * (int)spacing));
                    bldSpots[i, (j * (int)spacing)] = number;

                }
                i += (int)spacing;
                text = reader.ReadLine();
            }
        }
    }

    private float getLocationHeight(int i, int j)
    {
        // The y value is arbitray. should be higher than the highest point on the map.
        Vector3 loc = new Vector3(i, 750, j);
        return Terrain.activeTerrain.SampleHeight(loc);
       
    }
    private void determinePlacability(int i, int j)
    {        
        RaycastHit hitInfo;
        if(Physics.Raycast(new Vector3(i, 750, j), Vector3.down, out hitInfo, Mathf.Infinity, buildingsLayerMask)){
            bldSpots[i, j] = detectBuilding(hitInfo);
            return;
        }

        float[] heights = { getLocationHeight(i, j), getLocationHeight(i + (int)spacing, j), getLocationHeight(i, j + (int)spacing), getLocationHeight(i + (int)spacing, j + (int)spacing) };
        float max = heights[0];
        float min = heights[0];

        foreach (float x in heights)
        {
            if (x > max) { max = x;}
            if (x < min) { min = x;}
            if(x < 0)
            {
                bldSpots[i, j] = 0;
            }
        }
        if(min <= 46 && max <= 46) { 
            bldSpots[i, j] = -2;
            return;
        }

       
        //Check if the heights are on opposite sides of the square
        if ((heights[0]==max || heights[0]==min) && (heights[3] == min || heights[3] == max)  || (heights[1] == max || heights[1] == min) && (heights[2] == min || heights[2] == max))
        {
            float distance = Mathf.Sqrt(2) * spacing;
            if(Mathf.Rad2Deg * Mathf.Atan((max - min) / distance) <= 30){bldSpots[i, j] = -1;}
            else{ bldSpots[i, j] = 0;}
        }
        else
        {
            if (Mathf.Rad2Deg * Mathf.Atan((max - min) / spacing) <= 30) { bldSpots[i, j] = -1; }
            else { bldSpots[i, j] = 0; }
        }
    }
    public float getSpacing()
    {
        return spacing;
    }

    private int detectBuilding(RaycastHit hit)
    {
        if (hit.collider.gameObject.TryGetComponent<EnergyProducer>(out EnergyProducer energy))
        {
            return 1;
        }
        if (hit.collider.gameObject.TryGetComponent<Pylon>(out Pylon pylon))
        {
            return 2;
        }
        if (hit.collider.gameObject.TryGetComponent<Producer>(out Producer producer))
        {
            return 3;
        }
        if (hit.collider.gameObject.TryGetComponent<ProducerSpot>(out ProducerSpot producerSpot))
        {
            return -3;
        }

        return 0;
    }

}

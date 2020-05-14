using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;//TODO remove this 


/*
 * I'll explain the algorithm in detail in the Github wiki
 */
public class FlowField : MonoBehaviour
{
    int mapSize = 32;

    FlowGrid[][] subGrids;
    

    int targetX, targetY;


    public static FlowGrid testGrid;

    public void Start()
    {
        testGrid = new FlowGrid();
        testGrid.integrationField[10, 10] = 0;
        testGrid.dirtySquares.Add(Vector2Int.one * 10);
        

        for (int i = 0; i < FlowGrid.gridResolution; i++)
        {
            for (int j = 0; j < FlowGrid.gridResolution; j++)
            {
                if ((i > 5 || i < 3) && (j > 3 && j < 9))
                {
                    testGrid.costField[i, j] = 2048;
                }
            }
        }

        testGrid.compute();
    }

    public void Update()
    {
        for(int i = 0; i < FlowGrid.gridResolution; i++)
        {
            for(int j = 0; j < FlowGrid.gridResolution; j++)
            {
                Vector3 position = new Vector3(i + 0.5f, 0, j + 0.5f);
                Vector3 direction = FlowGrid.GetDirection(testGrid.directionField[i,j])/2;
                Debug.DrawRay(position, direction);
            }
        }
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < FlowGrid.gridResolution; i++)
        {
            for (int j = 0; j < FlowGrid.gridResolution; j++)
            {
                Vector3 position = new Vector3(i + 0.5f, 0, j + 0.5f);
                Handles.Label(position, testGrid.costField[i, j] + "");
                //Handles.Label(position + Vector3.right/4, testGrid.integrationField[i, j] + "");
            }
        }
    }

}

public class FlowGrid
{

    public static int gridResolution = 32;
    private static int[] directionLUT = {7,0,1,6,-1,2,5,4,3};
    /* 0 = up
     * ... clockwise rotation
     * 7 = upper left
     * -1 = no direction
     */

    public List<Vector2Int> dirtySquares;

    public int[,] costField;//TODO figure out what the best way to pull the cost field from the map.
    public int[,] integrationField;
    public int[,] directionField;

    public FlowGrid()
    {
        dirtySquares = new List<Vector2Int>();
        costField = new int[gridResolution,gridResolution];
        integrationField = new int[gridResolution,gridResolution];
        directionField = new int[gridResolution,gridResolution];
        for (int i = 0; i < gridResolution; i++)
        {
            for (int j = 0; j < gridResolution; j++)
            {
                integrationField[i, j] = 2*gridResolution*gridResolution;
                costField[i, j] = 1;
                directionField[i, j] = -1;
            }
        }
    }

    public void compute()
    {
        //Assign target squares before calling compute.
        while(dirtySquares.Count > 0)
        {
            Vector2Int current = dirtySquares[0];
            Debug.Log(current);
            dirtySquares.RemoveAt(0);
            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2Int next = current + new Vector2Int(i, j);
                    if (checkBounds(next.x,next.y))
                    {
                        int newValue = integrationField[current.x, current.y] + costField[next.x,next.y];
                        if(newValue < integrationField[next.x, next.y])
                        {
                            integrationField[next.x, next.y] = newValue;
                            if(!dirtySquares.Contains(next))
                                dirtySquares.Add(next);
                        }
                    }
                }
            }
        }

        for (int x = 0; x < gridResolution; x++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                int min = integrationField[x,y];
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //Debug.Log(checkBounds(x + i, y + j));
                        if (checkBounds(x + i, y + j))
                        {
                            if (integrationField[x + i, y + j] < min)
                            {
                                min = integrationField[x + i, y + j];
                                //Debug.Log(min);
                            }
                        }
                    }
                }
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (checkBounds(x + i, y + j))
                        {
                            if ((i + j + 2) % 2 == 0)
                            {
                                if (integrationField[x + i, y + j] == min)
                                {
                                    directionField[x, y] = directionLUT[(i + 1) + (j + 1) * 3];
                                    //Debug.Log(directionField[x, y]);
                                }
                            }
                        }
                    }
                }
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (checkBounds(x + i, y + j))
                        {
                            if ((i + j + 2) % 2 == 1)
                            {
                                if (integrationField[x + i, y + j] == min)
                                {
                                    directionField[x, y] = directionLUT[(i + 1) + (j + 1) * 3];
                                    //Debug.Log(directionField[x, y]);
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    private bool checkBounds(int i, int j)
    {
        return (i > 0) && (j > 0) && (i < gridResolution) && (j < gridResolution);
    }

    public static Vector3 GetDirection(int i)
    {
        switch(i)
        {
            case 2:
                return Vector3.right;
            case 3:
                return (Vector3.right + Vector3.forward).normalized;
            case 4:
                return (Vector3.forward);
            case 5:
                return (-Vector3.right + Vector3.forward).normalized;
            case 6:
                return -Vector3.right;
            case 7:
                return (-Vector3.right - Vector3.forward).normalized;
            case 0:
                return (-Vector3.forward);
            case 1:
                return (Vector3.right - Vector3.forward).normalized;


        }
        return Vector3.zero;
    }
}

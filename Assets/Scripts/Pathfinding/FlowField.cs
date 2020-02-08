using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * I'll explain the algorithm in detail in the Github wiki
 */
public class FlowField : MonoBehaviour
{
    int mapSize = 32;

    FlowGrid[][] subGrids;
    

    int targetX, targetY;
}

public class FlowGrid
{

    public static int gridResolution = 24;
    private static int[] directionLUT = {7,0,1,6,-1,2,5,4,3};
    /* 0 = up
     * ... clockwise rotation
     * 7 = upper left
     * -1 = no direction
     */

    List<Vector2Int> dirtySquares;

    int[,] costField;//TODO figure out what the best way to pull the cost field from the map.
    int[,] integrationField;
    int[,] directionField;

    public FlowGrid()
    {
        costField = new int[gridResolution,gridResolution];
        integrationField = new int[gridResolution,gridResolution];
        directionField = new int[gridResolution,gridResolution];
        for (int i = 0; i < gridResolution; i++)
        {
            for (int j = 0; j < gridResolution; j++)
            {
                integrationField[i, j] = 2*gridResolution*gridResolution;
                integrationField[i, j] = -1;
            }
        }
    }

    public void compute()
    {
        //Assign target squares before calling compute.
        while(dirtySquares.Count > 0)
        {
            Vector2Int current = dirtySquares[0];
            dirtySquares.RemoveAt(0);
            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2Int next = current + new Vector2Int(i, j);
                    if (checkBounds(next.x,next.y))
                    {
                        int newValue = integrationField[current.x, current.y] + costField[current.x + i,current.y + j];
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
                int min = -1;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {

                        if(integrationField[x+i,y+j] < min && !((i ==0) && (j==0)))
                        {
                            min = integrationField[x + i, y + j];
                        }
                    }
                }
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (integrationField[x + i, y + j] == min && !((i == 0) && (j == 0)))
                        {
                            directionField[x, y] = directionLUT[i+1+j+1];
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
}

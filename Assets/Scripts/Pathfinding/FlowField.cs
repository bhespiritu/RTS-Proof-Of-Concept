using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * I'll explain the algorithm in detail in the Github wiki
 */
public class FlowField : MonoBehaviour
{
    int mapSize = 32;

    FlowGrid[][] subGrids;
    LinkedList<int>[] adjacencyList;

    int targetX, targetY;
}

public class FlowGrid
{

    public static int gridResolution = 24;

    int[][] costField;//TODO figure out what the best way to pull the cost field from the map.
    int[][] integration_field;
    int[][] direction_field;

    public FlowGrid()
    {

    }

    public void compute()
    {
        //Assign target squares before calling compute.

    }
}

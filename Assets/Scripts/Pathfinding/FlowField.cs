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
    int mapSize = 16;

    int targetX, targetY;

    public static GridR grid;

    ChunkNode[,] mapChunks;

    public float chunkScale = 10;

    public static FlowGrid testGrid;
    public static PathRequest testRequest;

    private class ChunkNode
    {

        public ChunkNode(int x, int y)
        {
            gridPos = new Vector2Int(x, y);
        }

        public Vector2Int gridPos;

        public ChunkNode parent;

        public int gCost = 0;
        public int hCost = 0;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }

    public PathRequest requestPath(Vector2 start, Vector2 end)
    {
        //TODO generating portal maps

        PathRequest request = new PathRequest(chunkScale);

        int startChunkX = (int)(start.x / mapSize);
        int startChunkY = (int)(start.y / mapSize);

        int endChunkX = (int)(end.x / mapSize);
        int endChunkY = (int)(end.y / mapSize);

        ChunkNode currentNode = mapChunks[startChunkX, startChunkY];
        ChunkNode endNode = mapChunks[endChunkX, endChunkY];

        List<ChunkNode> path = new List<ChunkNode>();

        List<ChunkNode> openSet = new List<ChunkNode>();//TODO replace with Heap optimization
        HashSet<ChunkNode> closedSet = new HashSet<ChunkNode>();


        openSet.Add(currentNode);

        while(openSet.Count > 0)
        {
            ChunkNode node = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                ChunkNode other = openSet[i];
                if (other.fCost <= node.fCost)
                {
                    if (other.hCost < node.hCost)
                        node = other;
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == endNode)
            {
                ChunkNode trace = endNode;

                while(trace != currentNode)
                {
                    path.Add(trace);
                    trace = trace.parent;
                }
                path.Add(currentNode);
                break;
            }

            ChunkNode neighbor;
            if (node.gridPos.y < mapSize - 1)
            {
                neighbor = mapChunks[node.gridPos.x, node.gridPos.y + 1];
                if (!closedSet.Contains(neighbor))
                {//up
                    int newCost = node.gCost + 1;
                    if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = (neighbor.gridPos - endNode.gridPos).sqrMagnitude;
                        neighbor.parent = node;
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
            if (node.gridPos.x < mapSize - 1)
            {
                neighbor = mapChunks[node.gridPos.x + 1, node.gridPos.y];
                if (!closedSet.Contains(neighbor))
                {//right
                    int newCost = node.gCost + 1;
                    if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = (neighbor.gridPos - endNode.gridPos).sqrMagnitude;
                        neighbor.parent = node;
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            if (node.gridPos.y > 0)
            {
                neighbor = mapChunks[node.gridPos.x, node.gridPos.y - 1];
                if (!closedSet.Contains(neighbor))
                {//down
                    int newCost = node.gCost + 1;
                    if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = (neighbor.gridPos - endNode.gridPos).sqrMagnitude;
                        neighbor.parent = node;
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            if (node.gridPos.x > 0)
            {
                neighbor = mapChunks[node.gridPos.x - 1, node.gridPos.y];
                if (!closedSet.Contains(neighbor))
                {//left
                    int newCost = node.gCost + 1;
                    if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = (neighbor.gridPos - endNode.gridPos).sqrMagnitude;
                        neighbor.parent = node;
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

        }


        FlowGrid finalGrid = new FlowGrid();
        finalGrid.integrationField[(int)(end.x / FlowGrid.gridResolution), (int)(end.y / FlowGrid.gridResolution)] = 0;
        finalGrid.dirtySquares.Add(new Vector2Int((int)(end.x / FlowGrid.gridResolution), (int)(end.y / FlowGrid.gridResolution)));
        finalGrid.compute();
        finalGrid.chunkX = endChunkX;
        finalGrid.chunkY = endChunkY;
        request.chunkSteps.Add(new Vector2Int(endChunkX, endChunkY), finalGrid);

        FlowGrid lastGrid = finalGrid;
        for (int i = 1; i < path.Count; i++)
        {
            ChunkNode next = path[i-1];
            ChunkNode current = path[i];
            Vector2Int relative = next.gridPos - current.gridPos;


            //temp visualization
            var placeHolder = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Vector3 position = new Vector3(current.gridPos.x, 0, current.gridPos.y)*16 + new Vector3(8, 0, 8);
            placeHolder.transform.position = position*chunkScale;
            placeHolder.transform.localScale = Vector3.one * 1.6f * chunkScale;
            placeHolder.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, (float)i / path.Count);
           

            FlowGrid newGrid = new FlowGrid();
            if (relative.x == -1)//left
            {
                for(int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.integrationField[0, j] = lastGrid.integrationField[FlowGrid.gridResolution-1,j]+1;
                    newGrid.dirtySquares.Add(new Vector2Int(0, j));//TODO carryover from the previous grid.
                }
                newGrid.compute();
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.directionField[0, j] = 6;
                }
            }
            else if (relative.x == 1)//right
            {
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.integrationField[FlowGrid.gridResolution - 1, j] = lastGrid.integrationField[0, j]+1;
                    newGrid.dirtySquares.Add(new Vector2Int(FlowGrid.gridResolution - 1, j));
                }
                newGrid.compute();
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.directionField[FlowGrid.gridResolution - 1, j] = 2;
                }
            } else if (relative.y == 1)//up
            {
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.integrationField[j, FlowGrid.gridResolution - 1] = lastGrid.integrationField[j, 0]+1;
                    newGrid.dirtySquares.Add(new Vector2Int(j, FlowGrid.gridResolution - 1));
                }
                newGrid.compute();
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.directionField[j, FlowGrid.gridResolution - 1] = 4;
                }
            } else if (relative.y == -1)//down
            {
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.integrationField[j,0] = lastGrid.integrationField[j, FlowGrid.gridResolution - 1]+1;
                    newGrid.dirtySquares.Add(new Vector2Int(j,0));
                }
                newGrid.compute();
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    newGrid.directionField[j,0] = 0;
                }
            } else
            {
                Debug.LogError("Invalid Path Found");
            }

            newGrid.chunkX = current.gridPos.x;
            newGrid.chunkY = current.gridPos.y;
            request.chunkSteps.Add(current.gridPos, newGrid);

            lastGrid = newGrid;
        }
        

        var placeHolder2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Vector3 position2 = new Vector3(endChunkX, 0, endChunkY) * 16 + new Vector3(8, 0, 8);
        placeHolder2.transform.position = position2*chunkScale;
        placeHolder2.transform.localScale = Vector3.one * 1.6f * chunkScale;
        placeHolder2.GetComponent<Renderer>().material.color = Color.red;

        return request;
    }


    public void Awake()
    {
        mapChunks = new ChunkNode[mapSize, mapSize];
        for(int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                mapChunks[i, j] = new ChunkNode(i, j);
            }
        }

        testGrid = new FlowGrid();
        for(int i = 0; i < FlowGrid.gridResolution; i++)
        {
            testGrid.integrationField[i, 15] = 0;
            testGrid.dirtySquares.Add(new Vector2Int(i,15));
        }

        grid = GameObject.FindObjectOfType<GridR>();
        testGrid.compute();

        testRequest = requestPath(Vector2.zero, new Vector2(32,32));
    }


    public void OnDrawGizmos()
    {
        //return;
        foreach(KeyValuePair<Vector2Int, FlowGrid> pair in testRequest.chunkSteps)
        {
            for (int i = 0; i < FlowGrid.gridResolution; i++)
            {
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    Vector3 dir = FlowGrid.GetDirection(pair.Value.directionField[i, j]);
                    //Debug.DrawRay(new Vector3(i, 0, j)*10 + 10*FlowGrid.gridResolution*new Vector3(pair.Key.x,0,pair.Key.y),Vector3.up * pair.Value.integrationField[i,j]/50);
                    Handles.Label(new Vector3(i, 0, j) * 10 + 10 * FlowGrid.gridResolution * new Vector3(pair.Key.x, 0, pair.Key.y), pair.Value.integrationField[i, j] + "");
                    Debug.DrawRay(new Vector3(i, 0, j) * 10 + 10 * FlowGrid.gridResolution * new Vector3(pair.Key.x,0, pair.Key.y), dir*5);
                }
            }
        }
    }



}


public class PathRequest
{
    public Dictionary<Vector2Int,FlowGrid> chunkSteps = new Dictionary<Vector2Int, FlowGrid>();

    float mapScale;

    public PathRequest(float scale)
    {
        mapScale = scale;
    }

    public Vector3 getDirection(float x, float y)
    {
        float sx = x / mapScale;
        float sy = y / mapScale;
        int ChunkX = (int)(sx / FlowGrid.gridResolution);
        int ChunkY = (int)(sy / FlowGrid.gridResolution);
        int gridX = (int)(sx % FlowGrid.gridResolution);
        int gridY = (int)(sy % FlowGrid.gridResolution);

        FlowGrid grid = chunkSteps[new Vector2Int(ChunkX, ChunkY)];

        return FlowGrid.GetDirection(grid.directionField[gridX, gridY]);
    }
}

public class FlowGrid
{
    public int chunkX, chunkY;

    public static int gridResolution = 16;
    private static int[] directionLUT = {7,0,1,6,-1,2,5,4,3};
    /* 0 = up
     * ... clockwise rotation
     * 7 = upper left
     * -1 = no direction
     */

    public List<Vector2Int> dirtySquares;
    
    public int[,] integrationField;
    public int[,] directionField;

    public GridR grid;

    public FlowGrid()
    {
        dirtySquares = new List<Vector2Int>();
        integrationField = new int[gridResolution,gridResolution];
        directionField = new int[gridResolution,gridResolution];
        reset();
    }

    public void reset()
    {
        for (int i = 0; i < gridResolution; i++)
        {
            for (int j = 0; j < gridResolution; j++)
            {
                integrationField[i, j] = 256;
                directionField[i, j] = -1;
            }
        }
    }

    public void compute()//TODO: Implement more sophisticated systems such as the LOS Pass.
    {
        //Assign target squares before calling compute.
        while(dirtySquares.Count > 0)
        {
            Vector2Int current = dirtySquares[0];
            //Debug.Log(current);
            dirtySquares.RemoveAt(0);
            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2Int next = current + new Vector2Int(i, j);
                    if (checkBounds(next.x,next.y))
                    {
                        int cost = getCost(next.x, next.y);
                        if (cost >= 255) continue;
                        int newValue = integrationField[current.x, current.y] + cost;
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
        return (i >= 0) && (j >= 0) && (i < gridResolution) && (j < gridResolution);
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

    public int getCost(int x, int y)
    {
        return 1;
        int bld = FlowField.grid.getBuilding((x + chunkX*gridResolution)*10, (y + chunkY * gridResolution) *10);
        Debug.Log(new Vector2((x + chunkX * gridResolution) * 10, (y + chunkY * gridResolution) * 10));
        Debug.Log(bld);
        if (bld == 0) return 2;
        return 1;
    }
}

public class PortalGraph
{

    public class PortalNode
    {
        int size;
        int startIndex;
        int side;//0 = top, 1 = right, 2 = down, left = 3

        int gridX, gridY;//the gridspace it connects that is farther to the lower left.

        bool enabled = true;//in case someone blocks off an area with buildings.
    }

    Dictionary<Vector2Int, PortalNode> graph;

    public PortalNode[] GetNodesAt(Vector2Int input)
    {
        return GetNodesAt(input.x, input.y);
    }

    public PortalNode[] GetNodesAt(int x, int y)
    {
        int checkboardColor = (x + y) % 2;
        return null;
    }

}

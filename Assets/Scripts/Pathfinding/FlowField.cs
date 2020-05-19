using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;//TODO remove this 


/*
 * Author: Noah Espiritu
 * TODO: refactor everything in this. There are multiple types of grids involved, so the variable names are a bit confusing.
 * /


/*
 * I'll explain the algorithm in detail in the Github wiki
 */
public class FlowField : MonoBehaviour
{
    int mapSize = 16;

    AStarPathfinding pathfinding = new AStarPathfinding();

    public static PortalGraph navGraph;

    public static GridR grid;

    GridDataChunk[,] mapData;

    public float chunkScale = 10;
    
    public static PathRequest testRequest;
    

    private class ChunkNode : AStarNode
    {

        public ChunkNode(int x, int y, int size)
        {
            gridPos = new Vector2Int(x, y);
            mapSize = size;
        }

        public Vector2Int gridPos;

        int mapSize;

        public override Vector2 getPosition()
        {
            return gridPos;
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
        //TODO add in detection for which start nodes it has access to.

        pathfinding.calculatePath(navGraph, navGraph.nodes[navGraph.associatedNodes[startChunkX, startChunkY][0]], navGraph.nodes[navGraph.associatedNodes[endChunkX, endChunkY][0]]);
        List<AStarNode> path = pathfinding.path;

        FlowGrid finalGrid = new FlowGrid();
        finalGrid.integrationField[(int)(end.x % FlowGrid.gridResolution), (int)(end.y % FlowGrid.gridResolution)] = 0;
        finalGrid.dirtySquares.Add(new Vector2Int((int)(end.x % FlowGrid.gridResolution), (int)(end.y % FlowGrid.gridResolution)));
        finalGrid.compute();
        finalGrid.chunkX = endChunkX;
        finalGrid.chunkY = endChunkY;
        request.chunkSteps.Add(new Vector2Int(endChunkX, endChunkY), finalGrid);

        FlowGrid lastGrid = finalGrid;
        for (int i = 1; i < path.Count; i++)
        {
            PortalGraph.PortalNode next = (PortalGraph.PortalNode) path[i-1];
            PortalGraph.PortalNode current = (PortalGraph.PortalNode)path[i];

            Vector2Int currGrid = new Vector2Int(current.gridX, current.gridY);
            Vector2Int relative = (new Vector2Int(next.gridX, next.gridY) - currGrid);
            if (!request.chunkSteps.ContainsKey(currGrid))
            {
                //temp visualization
                var placeHolder = GameObject.CreatePrimitive(PrimitiveType.Plane);
                Vector3 position = new Vector3(currGrid.x, 0, currGrid.y) * 16 + new Vector3(8, 0, 8);
                placeHolder.transform.position = position * chunkScale;
                placeHolder.transform.localScale = Vector3.one * 1.6f * chunkScale;


                FlowGrid newGrid = new FlowGrid();
                if (relative.x < 0)//left
                {
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.integrationField[0, j] = lastGrid.integrationField[FlowGrid.gridResolution - 1, j] + 1;
                        newGrid.dirtySquares.Add(new Vector2Int(0, j));//TODO carryover from the previous grid.
                    }
                    newGrid.compute();
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.directionField[0, j] = 6;
                    }
                }
                else if (relative.x > 0)//right
                {
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.integrationField[FlowGrid.gridResolution - 1, j] = lastGrid.integrationField[0, j] + 1;
                        newGrid.dirtySquares.Add(new Vector2Int(FlowGrid.gridResolution - 1, j));
                    }
                    newGrid.compute();
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.directionField[FlowGrid.gridResolution - 1, j] = 2;
                    }
                }
                else if (relative.y > 0)//up
                {
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.integrationField[j, FlowGrid.gridResolution - 1] = lastGrid.integrationField[j, 0] + 1;
                        newGrid.dirtySquares.Add(new Vector2Int(j, FlowGrid.gridResolution - 1));
                    }
                    newGrid.compute();
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.directionField[j, FlowGrid.gridResolution - 1] = 4;
                    }
                }
                else if (relative.y < 0)//down
                {
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.integrationField[j, 0] = lastGrid.integrationField[j, FlowGrid.gridResolution - 1] + 1;
                        newGrid.dirtySquares.Add(new Vector2Int(j, 0));
                    }
                    newGrid.compute();
                    for (int j = 0; j < FlowGrid.gridResolution; j++)
                    {
                        newGrid.directionField[j, 0] = 0;
                    }
                }
                else
                {
                    Debug.LogError("Invalid Path Found");
                }

                newGrid.chunkX = currGrid.x;
                newGrid.chunkY = currGrid.y;

                request.chunkSteps.Add(currGrid, newGrid);

                lastGrid = newGrid;
            }
        }
        

        var placeHolder2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Vector3 position2 = new Vector3(endChunkX, 0, endChunkY) * 16 + new Vector3(8, 0, 8);
        placeHolder2.transform.position = position2*chunkScale;
        placeHolder2.transform.localScale = Vector3.one * 1.6f * chunkScale;
        placeHolder2.name = "End";

        return request;
    }


    public void Awake()
    {
        grid = FindObjectOfType<GridR>();//TODO replace
        mapData = new GridDataChunk[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                mapData[i, j] = new GridDataChunk(FlowGrid.gridResolution, i, j);
            }
        }

        navGraph = generateGraph();

        pathfinding = new AStarPathfinding();

        testRequest = requestPath(Vector2.zero, new Vector2(4,40));
    }


    public void OnDrawGizmos()
    {

        foreach (KeyValuePair<Vector2Int, FlowGrid> pair in testRequest.chunkSteps)
        {

            for (int i = 0; i < FlowGrid.gridResolution; i++)
            {
                for (int j = 0; j < FlowGrid.gridResolution; j++)
                {
                    Vector3 dir = FlowGrid.GetDirection(pair.Value.directionField[i, j]);
                    //Debug.DrawRay(new Vector3(i, 0, j)*10 + 10*FlowGrid.gridResolution*new Vector3(pair.Key.x,0,pair.Key.y),Vector3.up * pair.Value.integrationField[i,j]/50);
                    Handles.Label(new Vector3(i, 0, j) * 10 + 10 * FlowGrid.gridResolution * new Vector3(pair.Key.x, 0, pair.Key.y), pair.Value.integrationField[i, j] + "");
                    Debug.DrawRay(new Vector3(i, 0, j) * 10 + 10 * FlowGrid.gridResolution * new Vector3(pair.Key.x, 0, pair.Key.y), dir * 5);
                }
            }
        }
        

        foreach(PortalGraph.PortalNode node in navGraph.nodes)
        {
            bool horiz = node.isHorizontal;
            Vector2 nodePos = node.getPosition();
            Vector3 position = new Vector3(nodePos.x, 0 ,nodePos.y);
            Vector3 size = Vector3.one * 5;
            if(horiz)
            {
                size += Vector3.right * (node.size) * 10;
            } else
            {
                size += Vector3.forward * (node.size) * 10;
            }
            Gizmos.DrawSphere(position, 6);
            Gizmos.DrawCube(position, size);

            foreach(int i in node.connectedNodes)
            {
                Vector2 nodePos2 = navGraph.nodes[i].getPosition();
                Vector3 otherPos = new Vector3(nodePos2.x, 0, nodePos2.y);
                Debug.DrawLine(position, otherPos);
            }
        }
    }

    public PortalGraph generateGraph()
    {
        PortalGraph graph = new PortalGraph(mapSize);

        for(int x = 0; x < 2*mapSize - 2; x++)
        {
            bool horiz = x % 2 == 0;
            for (int y = 0; y < mapSize - 1 + (horiz ? -1 : 0); y++)
            {
                bool sideA;
                bool sideB;

                if(horiz)//TODO simplify these lines of code
                {
                    sideA = FlowField.grid.getBuilding((x/2 * FlowGrid.gridResolution * 10), (y * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution - 1) * 10) == -1;
                    sideB = FlowField.grid.getBuilding((x/2 * FlowGrid.gridResolution * 10), (y * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution) * 10) == -1;
                } else
                {
                    sideA = FlowField.grid.getBuilding(((x-1)/2 * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution - 1) * 10, y * FlowGrid.gridResolution * 10) == -1;
                    sideB = FlowField.grid.getBuilding(((x-1)/2 * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution) * 10, y * FlowGrid.gridResolution * 10) == -1;
                }


                bool foundPortal = sideA && sideB;

                int startIndex = -1;
                int size = -1;

                if(foundPortal)
                {
                    startIndex = 0;
                    size = 1;
                } 

                for(int i = 1; i < FlowGrid.gridResolution; i++)
                {
                    if (horiz)
                    {
                        sideA = FlowField.grid.getBuilding((x/2 * FlowGrid.gridResolution * 10) + i*10, (y * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution - 1) * 10) == -1;
                        sideB = FlowField.grid.getBuilding((x/2 * FlowGrid.gridResolution * 10) + i*10, (y * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution) * 10) == -1;
                    }
                    else
                    {
                        sideA = FlowField.grid.getBuilding(((x - 1)/2 * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution - 1) * 10, y * FlowGrid.gridResolution * 10 + i*10) == -1;
                        sideB = FlowField.grid.getBuilding(((x - 1)/2 * FlowGrid.gridResolution * 10) + (FlowGrid.gridResolution) * 10, y * FlowGrid.gridResolution * 10 + i*10) == -1;
                    }

                    if (sideA && sideB)
                    {
                        if(foundPortal)
                        {
                            size++;
                        } else
                        {
                            foundPortal = true;
                            startIndex = i;
                            size = 1;
                        }
                    } else
                    {
                        if(foundPortal)
                        {
                            foundPortal = false;

                            graph.AddPortal(x,y,size,startIndex);

                        }
                    }
                }
                if(foundPortal)
                {
                    graph.AddPortal(x, y, size, startIndex);
                }
            }
        }

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                List<int> portalIndices = graph.associatedNodes[x, y];
                for (int i = 0; i < portalIndices.Count; i++)
                {
                    int startX;
                    int startY;
                    int startIndex = portalIndices[i];

                    PortalGraph.PortalNode start = graph.nodes[i];
                    bool isHoriz = start.isHorizontal;
                    if(isHoriz)
                    {
                        startX = start.startIndex;
                        startY = (start.side ? 0 : FlowGrid.gridResolution - 1);
                    } else
                    {
                        startY = start.startIndex;
                        startX = (start.side ? 0 : FlowGrid.gridResolution - 1);
                    }
                    for (int j = i+1; j < portalIndices.Count; j++)
                    {
                        int targetX;
                        int targetY;
                        int targetIndex = portalIndices[j];

                        PortalGraph.PortalNode target = graph.nodes[j];
                        isHoriz = target.isHorizontal;
                        if (isHoriz)
                        {
                            targetX = target.startIndex;
                            targetY = (target.side ? 0 : FlowGrid.gridResolution - 1);
                        }
                        else
                        {
                            targetY = target.startIndex;
                            targetX = (target.side ? 0 : FlowGrid.gridResolution - 1);
                        }
                        Debug.Log(startX + " " + startY + " " + targetX + " " + targetY);
                        bool foundPath = pathfinding.calculatePath(mapData[x,y], mapData[x,y].AStarGrid[startX,startY], mapData[x, y].AStarGrid[targetX, targetY]);
                        if(foundPath)
                        {
                            graph.Connect(startIndex, targetIndex);
                        }

                    }
                }
               
            }
        }

        return graph;
    }

  
}

//TODO: add a way to detect when two portals are cut off
public class PortalGraph : NodeData
{

    int mapSize;

    public PortalGraph(int mapSize)
    {
        associatedNodes = new List<int>[mapSize,mapSize];
        nodes = new List<PortalNode>();
        this.mapSize = mapSize;
        for(int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                associatedNodes[x, y] = new List<int>();
            }
        }
    }

    public class PortalNode : AStarNode
    {
        public int size;
        public int startIndex;

        public HashSet<int> connectedNodes = new HashSet<int>();

        public int gridX, gridY;
        public bool side;//false means it's on the upper/right-er end of the square

        public bool isHorizontal
        {
            get
            {
                return gridX % 2 == 0;
            }
        }


        public PortalNode(int x, int y, int size, int start, bool side)
        {
            gridX = x;
            gridY = y;
            this.size = size;
            startIndex = start;
            this.side = side;
        }



        public override Vector2 getPosition()
        {
            bool horiz = isHorizontal;
            Vector2 position = new Vector3(16 * (gridX / 2 + (horiz ? 0 : 1) - (side && !horiz ? 1 : 0)), 16 * (gridY + (horiz ? 1 : 0) - (side && horiz ? 1 : 0))) * 10;//I'm too lazy to simplify this
            if (horiz)
            {
                position += Vector2.right * 10 * (startIndex + size / 2);
                position += Vector2.up * (side ? 1 : -1) * 5;
            }
            else
            {
                position += Vector2.up * 10 * (startIndex + size / 2);
                position += Vector2.right * (side ? 1 : -1) * 5;
            }
            return position;
        }
    }

    public List<int>[,] associatedNodes;//stores all nodes that are connected to some gridspace

    public List<PortalNode> nodes;

    public PortalNode[] GetNodesAt(Vector2Int input)
    {
        return GetNodesAt(input.x, input.y);
    }

    public PortalNode[] GetNodesAt(int x, int y)
    {
        List<int> nodeIndices = associatedNodes[x, y];
        PortalNode[] output = new PortalNode[nodeIndices.Count];

        int i = 0;
        foreach(int nodeIndex in nodeIndices)
        {
            output[i++] = nodes[nodeIndex];
        }
        return output;
    }


    public void AddPortal(int x, int y, int size, int start)
    {
        PortalNode node = new PortalNode(x, y, size, start, false);
        int index = nodes.Count;
        nodes.Add(node);
        

        if (x % 2 == 0)
        {
            associatedNodes[x, y].Add(index);
            node = new PortalNode(x, y+1, size, start, true);
            index++;
            nodes.Add(node);
            associatedNodes[x, y+1].Add(index);
        } else
        {
            associatedNodes[x-1, y].Add(index);
            node = new PortalNode(x+2, y, size, start, true);
            index++;
            nodes.Add(node);
            associatedNodes[x+1, y].Add(index);
        }

        Connect(index, index - 1);
    }

    private void AddNode(int x, int y, int size, int start, bool side)
    {
        PortalNode node = new PortalNode(x,y,size,start, side);
        int index = nodes.Count;
        nodes.Add(node);
    }

    public void Connect(int i, int j)
    {
        PortalNode nodeA = nodes[i];
        PortalNode nodeB = nodes[j];
        nodeA.connectedNodes.Add(j);
        nodeB.connectedNodes.Add(i);
    }

    public string toString()
    {
        return "";
    }

    public void readFrom(String s)
    {

    }

    public override void getNeighbors(AStarNode node, ref List<AStarNode> neighbors)
    {
        PortalNode pNode = (PortalNode)node;
        foreach(int i in pNode.connectedNodes)
        {
            neighbors.Add(nodes[i]);
        }
    }

    public override void resetNodes()
    {
        foreach(PortalNode node in nodes)
        {
            node.gCost = 0;
            node.hCost = 0;
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
        //return 1;//undo once I figure out why Roger's code isn't working
        int bld = FlowField.grid.getBuilding((x + chunkX*gridResolution)*10, (y + chunkY * gridResolution) * 10);
        //Debug.Log((x + chunkX * gridResolution) * 10 + " " + (y + chunkY * gridResolution) * 10  + " " + bld);
        if (bld == 0) return 256;
        return 1;
    }

    

}

public class GridDataChunk : NodeData
{

    int gridResolution;

    public FlowGridNode[,] AStarGrid;

    int chunkX, chunkY;

    public GridDataChunk(int res, int cx, int cy)
    {
        chunkX = cx;
        chunkY = cy;
        gridResolution = res;
        AStarGrid = new FlowGridNode[res, res];
        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                AStarGrid[x, y] = new FlowGridNode(x, y);
            }
        }
        
    }

    public override void getNeighbors(AStarNode node, ref List<AStarNode> neighbors)
    {
        FlowGridNode fgNode = (FlowGridNode)node;
        int x = fgNode.x;
        int y = fgNode.y;

        //TODO: add in obstacle detection when I figure out roger's code
        if (x > 0)
        {
            if(FlowField.grid.getBuilding((x - 1 + chunkX * gridResolution) * 10, (y + chunkY * gridResolution) * 10) == -1)
                neighbors.Add(AStarGrid[x - 1, y]);
        }
        if (y > 0)
        {
            if (FlowField.grid.getBuilding((x + chunkX * gridResolution) * 10, (y - 1 + chunkY * gridResolution) * 10) == -1)
                neighbors.Add(AStarGrid[x, y - 1]);
        }
        if (x < gridResolution - 1)
        {
            if (FlowField.grid.getBuilding((x + 1 + chunkX * gridResolution) * 10, (y + chunkY * gridResolution) * 10) == -1)
                neighbors.Add(AStarGrid[x + 1, y]);
        }
        if (y < gridResolution - 1)
        {
            if (FlowField.grid.getBuilding((x + chunkX * gridResolution) * 10, (y + 1 + chunkY * gridResolution) * 10) == -1)
                neighbors.Add(AStarGrid[x, y + 1]);
        }
    }


    public override void resetNodes()
    {
        for (int i = 0; i < gridResolution; i++)
        {
            for (int j = 0; j < gridResolution; j++)
            {
                FlowGridNode node = AStarGrid[i, j];
                node.gCost = 0;
                node.hCost = 0;
            }
        }
    }

    public class FlowGridNode : AStarNode
    {

        public int x, y;

        public FlowGridNode(int x, int y)
        {
            this.x = x;
            this.y = y;

        }


        public override Vector2 getPosition()
        {
            return new Vector2(x, y);
        }
    }
}


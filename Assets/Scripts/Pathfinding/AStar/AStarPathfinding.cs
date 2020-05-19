﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Noah Espiritu


//A paper recommends that we store previous results somehow to form a "merging" A* algorithm
//I don't know what that means, so I'm just gonna hold off on that right now and make each iteration standalone
public class AStarPathfinding
{
    public List<AStarNode> path;

    public List<AStarNode> openSet;//TODO replace with Heap for optimization
    public HashSet<AStarNode> closedSet;

    private List<AStarNode> neighbors;
   
    public AStarPathfinding()
    {
        path = new List<AStarNode>();
        openSet = new List<AStarNode>();
        closedSet = new HashSet<AStarNode>();
        neighbors = new List<AStarNode>();
    }

    public bool calculatePath(NodeData d, AStarNode start, AStarNode last)
    {
        path.Clear();
        openSet.Clear();
        closedSet.Clear();
        d.resetNodes();

        Vector2 target = last.getPosition();

        openSet.Add(start);
        bool foundPath = false;
        while(openSet.Count > 0)
        {
            AStarNode node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                AStarNode other = openSet[i];
                if (other.fCost <= node.fCost)
                {
                    if (other.hCost < node.hCost)
                        node = other;
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == last)
            {
                AStarNode trace = last;

                while (trace != start)
                {
                    path.Add(trace);
                    trace = trace.parent;
                }
                path.Add(start);
                foundPath = true;
                break;
            }

            neighbors.Clear();
            d.getNeighbors(node, ref neighbors);
            foreach(AStarNode neighbor in neighbors)
            {
                if (!closedSet.Contains(neighbor))
                {
                    float newCost = node.gCost + 1;
                    if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = (neighbor.getPosition() - target).sqrMagnitude;
                        neighbor.parent = node;
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }

            }

         

        }


        return foundPath;
    }


}


public abstract class AStarNode
{
    public abstract Vector2 getPosition();
    

    public AStarNode parent;

    public float gCost = 0;
    public float hCost = 0;

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}


public abstract class NodeData
{
    public abstract void resetNodes();
    public abstract void getNeighbors(AStarNode node, ref List<AStarNode> neighbors);
}
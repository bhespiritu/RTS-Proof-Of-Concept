﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * TODO: Add in periodic checks to clean out stale or empty groups.
 * */
//Author: Noah Espiritu
[RequireComponent(typeof(FlowFieldHandler))]
public class PathfindingManager : MonoBehaviour
{

    public void Awake()
    {
        RoundManager.OnRoundTick += OnTick;
        if(!INSTANCE || INSTANCE == null)
        {
            INSTANCE = this;
        } else
        {
            Debug.Log("Attempt to create duplicate PathfindingManager");
        }
    }

    public static PathfindingManager INSTANCE;

    List<UnitGroup> groups = new List<UnitGroup>();

    public float avoidanceWeight = 1;
    public float speed = 10;

    public FlowFieldHandler flowFieldHandler;

    public void Start()
    {
        flowFieldHandler = GetComponent<FlowFieldHandler>();

    }

    public UnitGroup formGroup()
    {
        UnitGroup group = new UnitGroup();
        groups.Add(group);
        return group;
    }

    public void OnTick()
    {
        foreach(UnitGroup g in groups)
        {
            UpdateGroupMovement(g);
        }
    }

    public void UpdateGroupMovement(UnitGroup group)
    {
        if(group.pathfindingData == null)
        {
            Debug.Log("A Unit group is attempting move without any initialized pathfinding");
            return;
        }

        List<Unit> units = group.associatedUnits;
        Vector3[] futurePositions = new Vector3[units.Count];
        bool[] isMoving = new bool[units.Count];
        //calculate the future positions of each unit based on where they want to go
        for(int i = 0; i < units.Count; i++)
        {
            Unit u = units[i];
            futurePositions[i] = u.transform.position + group.desiredUnitDirection(i)*Time.fixedDeltaTime;
            futurePositions[i].y = 0;
            isMoving[i] = group.desiredUnitDirection(i).sqrMagnitude > 0;
        }

        //see if any units will collide in the future
        for (int i = 0; i < units.Count; i++)
        {
            Unit priorityUnit = units[i];
            Vector3 newVelocity = group.desiredUnitDirection(i);
            for(int j = 0; j < units.Count; j++)
            {
                if(i != j)
                {
                    Unit otherUnit = units[j];
                    Vector3 diff = futurePositions[i] - futurePositions[j];
                    float dist = diff.sqrMagnitude;
                    diff = diff.normalized;
                    float scale = (1 - Mathf.Clamp01(dist/(UnitGroup.soft_unit_size*2)));

                    newVelocity += diff * scale * avoidanceWeight;
                }
            }
            newVelocity.y = 0;
            Vector3 nextPos = priorityUnit.transform.position;
            nextPos += newVelocity * Time.fixedDeltaTime * speed;
            nextPos.y = Terrain.activeTerrain.SampleHeight(nextPos)+ 5;
            priorityUnit.transform.position = nextPos;
        }
    }


}

public class UnitGroup
{

    public static float soft_unit_size = 30; //the amount of social distance each unit wants as a radius. thats a corona joke for ya.
    public static float hard_unit_size = 15; //how far must they be for them to be touching.

    public List<Unit> associatedUnits = new List<Unit>(); //first object is leader, lower index means higher priority
    public PathRequest pathfindingData;

    public Vector2 targetPosition;

    private List<Vector2> unitPositions = new List<Vector2>();

    public UnitGroup()
    {
    }

    public Vector3 desiredUnitDirection(int i)
    {
        Unit u = associatedUnits[i];
        Vector3 pos = u.transform.position;
        return pathfindingData.getDirection(pos.x,pos.z);
    }

    public void UpdatePathfinding()
    {
        //TODO Detect if the new target requires a total recalculation
        unitPositions.Clear();
        foreach(Unit u in associatedUnits)
        {
            unitPositions.Add(new Vector2(u.transform.position.x, u.transform.position.z)/10);
        }
        pathfindingData = PathfindingManager.INSTANCE.flowFieldHandler.requestPath(unitPositions, targetPosition/10); //replace the magic numbers with the spacing constant in Grid R
    }



    public void AddUnit(Unit u)
    {
        if(u.associatedPathfindingGroup != null)
        {
            u.associatedPathfindingGroup.RemoveUnit(u);
        }
        u.associatedPathfindingGroup = this;
        associatedUnits.Add(u);
    }

    public void RemoveUnit(Unit u)
    {
        u.associatedPathfindingGroup = null;
        associatedUnits.Remove(u);
    }


}




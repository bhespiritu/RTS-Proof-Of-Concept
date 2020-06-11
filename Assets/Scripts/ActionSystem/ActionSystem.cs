using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Noah Espiritu
//TODO: enable storing actions of multiple players
public class ActionSystem
{

    public RoundManager roundManager;
    public bool debug = false;

    public ActionSystem(RoundManager rm)
    {
        roundManager = rm;
        actionQueue = new Queue<IAction>();
    }

    Queue<IAction> actionQueue;

   public void performAction()
    {
        IAction action = null;

        if(actionQueue.Count > 0)
        {
            action = actionQueue.Dequeue();
        }

        if(action == null)
        {
            action = new NoOp();
        }

        action.ProcessAction(this);
        if (debug) Debug.Log(roundManager.tickCount + ": " + action.DebugSerialize());
    }

    public void addAction(IAction a)
    {
        actionQueue.Enqueue(a);
    }

}


public interface IAction
{
    void ProcessAction(ActionSystem ctx);
    string DebugSerialize();
}

public class NoOp : IAction
{
    public string DebugSerialize()
    {
        return "NoOp";
    }

    public void ProcessAction(ActionSystem ctx)
    {
        
    }
}


public class MoveOp : IAction
{
    public Vector2 targetPosition;

    public ulong[] targets;

    public string DebugSerialize()
    {
        return "Move to " + targetPosition;
    }

    public void ProcessAction(ActionSystem ctx)
    {
        Unit[] targetUnits = new Unit[targets.Length];
        for(int i = 0; i < targets.Length; i++)
        {
            targetUnits[i] = UnitManager.INSTANCE.unitLookup[targets[i]];
        }
        bool needsNewGroup = false;
        UnitGroup group = targetUnits[0].associatedPathfindingGroup;
        for(int i = 1; i < targetUnits.Length; i++)
        {
            if (group != targetUnits[i].associatedPathfindingGroup) needsNewGroup = true;
        }

        if(group != null)
        {
            needsNewGroup = (targets.Length != group.associatedUnits.Count);
        } else
        {
            needsNewGroup = true;
        }
        if(needsNewGroup)
        {
            group = PathfindingManager.INSTANCE.formGroup();
            foreach(Unit u in targetUnits)
            {
                group.AddUnit(u);
            }
        } 
        group.targetPosition = targetPosition;
        group.UpdatePathfinding();
        

    }
}

public class RequestUnitOp : IAction
{
    BuildingController targetBuilding;

    public string DebugSerialize()
    {
        throw new System.NotImplementedException();
    }

    public void ProcessAction(ActionSystem ctx)
    {
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Noah Espiritu
//TODO: enable storing actions of multiple players
public class ActionSystem
{
   

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
    }

    public void addAction(IAction a)
    {
        actionQueue.Enqueue(a);
    }

}


public interface IAction
{

    
    void ProcessAction(ActionSystem ctx);
}

public class NoOp : IAction
{
    public void ProcessAction(ActionSystem ctx)
    {
        
    }
}


public class Move : IAction
{
    public Vector2 targetPosition;

    public Unit[] targetUnits;

    public void ProcessAction(ActionSystem ctx)
    {
        bool needsNewGroup = false;
        UnitGroup group = targetUnits[0].associatedPathfindingGroup;
        for(int i = 1; i < targetUnits.Length; i++)
        {
            if (group != targetUnits[i].associatedPathfindingGroup) needsNewGroup = true;
        }

        if(group == null || needsNewGroup)
        {
            UnitGroup newGroup = new UnitGroup(null);// ctx.path; //TODO add in reference to PathfindingManager
            foreach(Unit u in targetUnits)
            {
                newGroup.AddUnit(u);
            }

            newGroup.calculateGroupPosition();

            group = newGroup;
        } 
        group.targetPosition = targetPosition;
        group.UpdatePathfinding();
        

    }
}

public class RequestUnit : IAction
{
    Building targetBuilding;

    public void ProcessAction(ActionSystem ctx)
    {
        
    }
}
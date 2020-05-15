using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{

    public GameObject debugObject;
    public Unit[] debugList;

    private UnitGroup debugGroup;

    public float avoidanceWeight = 1;
    public float speed = 10;

    public void Start()
    {
        debugGroup = new UnitGroup();
        debugList = (Unit[])FindObjectsOfType(typeof(Unit));
        foreach (Unit u in debugList) debugGroup.associatedUnits.Add(u);
    }

    public void FixedUpdate()
    {

        debugGroup.targetPosition = Vector3.one*10;
        UpdateGroupMovement(debugGroup);
    }

    public void UpdateGroupMovement(UnitGroup group)
    {
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

    public static float soft_unit_size = 20; //the amount of social distance each unit wants as a radius. thats a corona joke for ya.
    public static float hard_unit_size = 15; //how far must they be for them to be touching.

    public List<Unit> associatedUnits = new List<Unit>(); //first object is leader, lower index means higher priority
    FlowField pathfindingField;

    public Vector3 desiredUnitDirection(int i)
    {
        Unit u = associatedUnits[i];
        Vector3 pos = u.transform.position;
        return FlowField.testRequest.getDirection(pos.x,pos.z);
    }

    public Vector3 groupPosition;
    public float groupBearing;

    public Vector3 targetPosition;
    public UnitGroupState groupState;
}

public enum UnitGroupState
{
    Awaiting, Moving
}

public struct UnitCollision
{
    enum UnitCollisionType { A, B, C1, C2, C3, C4, D1, D2}

    UnitCollisionType type;

    bool isHard;

    Unit higher, lower;
}


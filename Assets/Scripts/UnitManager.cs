

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager
{

    public static UnitManager INSTANCE = new UnitManager();

    public Dictionary<ulong, Unit> unitLookup { get; private set; }


    private UnitManager()
    {
        unitLookup = new Dictionary<ulong, Unit>();
    }

    public static int UNIT_CAP = 500;//change it to whatever we feel like.


    


    public void SpawnUnit(Vector2 position)
    {
        Unit u = new Unit();
        u.uID = RoundManager.INSTANCE.requestUID;
        unitLookup.Add(u.uID, u);
    }

    public void AddUnit(Unit u)
    {
        Debug.Log("AddUnit should only be called for debug purposes. If you don't know why this is here, change your code");
        u.uID = RoundManager.INSTANCE.requestUID;
        unitLookup.Add(u.uID, u);
    }

    //Temp method while Busy Boy Kevin gets around to finishing his K-D tree.
    //returns a dictionary of units sorted by how far they are from the target position.
    public bool findUnitsNear(ref SortedDictionary<float,Unit> list, Vector3 target, float radius)
    {
        list.Clear();
        bool foundNearby = false;
        float sqrRad = radius * radius;
        foreach(KeyValuePair<ulong, Unit> unitPair in unitLookup)
        {
            float dist2 = (unitPair.Value.transform.position - target).sqrMagnitude;
            if(dist2 <= sqrRad)
            {
                foundNearby = true;
                list.Add(dist2,unitPair.Value);
            }
        }

        return foundNearby;
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager
{

    public static UnitManager INSTANCE = new UnitManager();

    public Dictionary<ulong, Unit> unitLookup { get; private set; }

    private List<Unit> unitList;
    private List<Vector3> unitPoints;


    private UnitManager()
    {
    }

    public static int UNIT_CAP = 500;//change it to whatever we feel like.


    


    public void SpawnUnit(Vector2 position)
    {
        Unit u = new Unit();
        u.uID = RoundManager.INSTANCE.requestUID;
        unitLookup.Add(u.uID, u);
    }

}

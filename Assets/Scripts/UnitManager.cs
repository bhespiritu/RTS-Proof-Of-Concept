

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager
{

    public static UnitManager INSTANCE = new UnitManager();

    private List<Unit> unitList;
    private List<Vector3> unitPoints;

    private static uint globalID = 0;


    private UnitManager()
    {
    }

    public static int UNIT_CAP = 500;//change it to whatever we feel like.


    


    public void SpawnUnit(Vector2 position)
    {
        Unit u = new Unit();
        //TODO add in spawn placement
        u.uID = globalID++;
    }

}

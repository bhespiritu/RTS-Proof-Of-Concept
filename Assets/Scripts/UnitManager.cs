

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager
{

    public static UnitManager INSTANCE = new UnitManager();

    private List<Unit> unitList;
    private List<Vector3> unitPoints;


    private UnitManager()
    {
    }

    public static int UNIT_CAP = 10000;//change it to whatever we feel like.


    


    public void AddUnit(Vector2 position)
    {

    }



    public List<int> searchRadius(Vector2 position, float radius)
    {
        var resultIndices = new List<int>();

        

        return resultIndices;
    }
}

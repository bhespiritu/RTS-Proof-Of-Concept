

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
       
       Vector3[] temp = { new Vector3(0, 0, 0), new Vector3(0, 0, 25), new Vector3(25, 0, 0), new Vector3(25, 0, 25) };
       unitPoints = new List<Vector3>(temp);
       unitList = new List<Unit>();//replace placeholder values later
    }

    public static int UNIT_CAP = 10000;//change it to whatever we feel like.
    private readonly int max_units_per_leaf = 15;


    //private KDTree unitTree = new KDTree();


    public void AddUnit()
    {

    }



    public List<int> searchRadius(Vector2 position, float radius)
    {
        var resultIndices = new List<int>();

        

        return resultIndices;
    }
}

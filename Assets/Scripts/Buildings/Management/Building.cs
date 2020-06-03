using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Building stores all the general methods used for buildings, then abstracts it to to the specific buildings handler
 */
public class Building : MonoBehaviour
{

    public List<GameObject> getElements()
    {
        if(TryGetComponent<FactoryBuildingHandler>(out FactoryBuildingHandler handler))
        {
            return handler.GetUIElements();
        }
        
        return null;
    }


}

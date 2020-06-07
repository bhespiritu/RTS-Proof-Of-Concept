using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUIPrefabs : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    public GameObject GetPanelPrefab()
    {
        return panel;
    }
}

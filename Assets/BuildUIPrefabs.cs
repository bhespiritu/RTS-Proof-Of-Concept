using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUIPrefabs : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject button;
    public GameObject GetPanelPrefab()
    {
        return panel;
    }

    public GameObject GetButtonPrefab()
    {
        return button;
    }
}

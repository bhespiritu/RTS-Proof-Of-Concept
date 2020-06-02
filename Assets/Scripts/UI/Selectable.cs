using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public static List<Selectable> allSelectable = new List<Selectable>();

    public GameObject selectionObject;

    public delegate void SelectEvent();
    public static event SelectEvent OnSelect;

    public delegate void DeselectEvent();
    public static event DeselectEvent OnDeselect;

    // Start is called before the first frame update
    void Awake()
    {
        allSelectable.Add(this);
    }

    public void Select()
    {
        OnSelect?.Invoke();
    }

    public void Deselect()
    {
        OnDeselect?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public enum SelectedType
    {
        NULL, UNIT, BUILDING
    }

    public SelectedType type;

    public static List<Selectable> allSelectable = new List<Selectable>();

    public GameObject selectionObject;

    public delegate void SelectEvent();
    public event SelectEvent OnSelect;

    public delegate void DeselectEvent();
    public event DeselectEvent OnDeselect;

    private Unit referencedUnit;
    private Building referencedBuilding;

    // Start is called before the first frame update
    void Awake()
    {
        allSelectable.Add(this);
        switch(type)
        {
            case SelectedType.UNIT:
                referencedUnit = GetComponent<Unit>();
                break;
            case SelectedType.BUILDING:
                throw new NotImplementedException();
        }
    }

    public void Select()
    {
        OnSelect?.Invoke();
    }

    public void Deselect()
    {
        OnDeselect?.Invoke();
    }

    public void OnDestroy()
    {
        OnSelect = null;
        OnDeselect = null;
        allSelectable.Remove(this);
    }

    public ulong getID()
    {
        switch(type)
        {
            case SelectedType.UNIT:
                return referencedUnit.uID;
            case SelectedType.BUILDING:
                throw new NotImplementedException();
        }
        return ulong.MaxValue;
    }
}

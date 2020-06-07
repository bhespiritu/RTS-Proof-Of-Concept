using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/*
* TODO:
* Make a method to check if the mouse is over a UI element
* Make selection only happen during selection mode
* Selection circle shows up under the list of objects in the cursors transfrom. This messes up the mesh changing. 
*   The cursor needs to make a list of what objects it wants to change the mesh of. Or to ignore the circle
*   Preferably the selection object shouldn't be added to cursor at all, as it won't be added when not in selection mode.
*/
public class MouseHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    Image selectionImage;
    Vector2 clickStart;

    Rect selectRect;

    public MouseMode mouseMode = MouseMode.Select;//TODO change behaviour based on mode.

    List<Selectable> selected = new List<Selectable>();

    public LayerMask selectionMask;

    public GameObject selectionPrefab;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            selectionImage.gameObject.SetActive(true);
            clickStart = eventData.position;
            selectRect = new Rect();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (eventData.position.x < clickStart.x)
            {
                selectRect.xMin = eventData.position.x;
                selectRect.xMax = clickStart.x;
            }
            else
            {
                selectRect.xMin = clickStart.x;
                selectRect.xMax = eventData.position.x;
            }

            if (eventData.position.y < clickStart.y)
            {
                selectRect.yMin = eventData.position.y;
                selectRect.yMax = clickStart.y;
            }
            else
            {
                selectRect.yMin = clickStart.y;
                selectRect.yMax = eventData.position.y;
            }

            selectionImage.rectTransform.offsetMin = selectRect.min;
            selectionImage.rectTransform.offsetMax = selectRect.max;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //CANCEL Select
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                DeselectAll();
            }

            selectionImage.gameObject.SetActive(false);
            foreach (Selectable s in Selectable.allSelectable)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(s.transform.position);
                if (selectRect.Contains(screenPos))
                {
                    Select(s);
                }
            }
        }
    }

    private RaycastHit clickCast = new RaycastHit();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandleLeftClick(eventData);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            HandleRightClick(eventData);
        }

        
    }

    private void HandleRightClick(PointerEventData eventData)
    {
        switch (mouseMode)
        {
            case MouseMode.Select:
                {

                    Ray clickWorldRay = Camera.main.ScreenPointToRay(eventData.position);
                    if (Physics.Raycast(clickWorldRay, out clickCast))
                    {
                        Vector3 target = clickCast.point;
                        Vector2 pathTarget = new Vector2(target.x, target.z);

                        MoveOp action = new MoveOp();

                        action.targets = new ulong[selected.Count];
                        for(int i = 0; i < selected.Count; i++)
                        {
                            action.targets[i] = selected[i].getID();
                        }
                        
                        action.targetPosition = pathTarget;
                        RoundManager.INSTANCE.actionSystem.addAction(action);
                    }
                    break;
                }
            case MouseMode.Placement:
                {
                    break;
                }
        }
    }

    private void HandleLeftClick(PointerEventData eventData)
    {
        switch (mouseMode)
        {
            case MouseMode.Select:
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        DeselectAll();
                    }

                    Ray clickWorldRay = Camera.main.ScreenPointToRay(eventData.position);
                    if (Physics.Raycast(clickWorldRay, out clickCast))
                    {
                        Selectable s = clickCast.transform.GetComponent<Selectable>();
                        if (s)
                        {
                            Select(s);
                        }
                    }
                    break;
                }
            case MouseMode.Placement:
                {
                    break;
                }
        }
    }

    public void Select(Selectable s)
    {
        if (!selected.Contains(s))
        {
            selected.Add(s);
            s.Select();

            GameObject selectionCircle = Instantiate(selectionPrefab, s.transform);
            selectionCircle.transform.localPosition = Vector3.up * -0.5f;
            s.selectionObject = selectionCircle;
        }
    }


    public void Deselect(Selectable s, bool removeItem = true)
    {
        s.Deselect();
        if(removeItem)
            selected.Remove(s);
        Destroy(s.selectionObject.gameObject);

    }

    public void DeselectAll()
    {
        foreach (Selectable s in selected)
        {
            Deselect(s, false);
        }
        selected.Clear();
    }

    //Things needed for placement mode
    public LayerMask placementLayerMask;
    public GridR grid;
    public BuildingPlacer bldPlacer;

    private void Update()
    {

        switch (mouseMode)
        {
            case (MouseMode.Placement):
                //Shoot a ray to the mouse
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 localUp = cursor.transform.up;
                Vector3 localForward = cursor.transform.forward;

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placementLayerMask))
                {
                    cursor.transform.LookAt(cursor.transform.position + localForward, localUp);

                    //Snap to the grid
                    cursor.transform.position = grid.getGridPoint(hitInfo.point);
                    //New offset so it fits on the grid spot
                    Vector3 pos = new Vector3(0f, 4.5f, 0f);
                    cursor.transform.position += pos;
                    localUp = hitInfo.normal;
                    cursor.transform.position += hitInfo.normal * 0.5f;

                    if (bldPlacer.checkPosition(cursor))
                    {
                        //Set the material of the cursor to the placeable color
                        mesh.material = placeable;
                        foreach (Transform child in cursor.transform)
                        {
                            child.gameObject.layer = 8;
                            childMesh = child.GetComponent<MeshRenderer>();
                            childMesh.material = placeable;
                        }
                    }
                    else
                    {
                        mesh.material = unplaceable;
                        foreach (Transform child in cursor.transform)
                        {
                            child.gameObject.layer = 8;
                            childMesh = child.GetComponent<MeshRenderer>();
                            childMesh.material = unplaceable;
                        }
                    }

                }

                //Rotate Left
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Vector3 temp = cursor.transform.forward;

                    temp.x = -cursor.transform.forward.z;
                    temp.z = cursor.transform.forward.x;
                    localForward = temp;
                    cursor.transform.forward = localForward;
                }
                //Rotate Right
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Vector3 temp = cursor.transform.forward;

                    temp.x = cursor.transform.forward.z;
                    temp.z = -cursor.transform.forward.x;
                    localForward = temp;
                    cursor.transform.forward = localForward;
                }


                if (Input.GetMouseButtonDown(0))
                {
                    //Replace the raycast with the ability to detect if the mouse is over a ui element
                    if (bldPlacer.checkPosition(cursor) && Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placementLayerMask))
                    {
                        bldPlacer.PlaceBuildingNear(cursor);
                    }
                }
                break;
        }
    }

    private void changeCursor(GameObject prefab)
    {
        Destroy(cursor);
        cursor = Instantiate(prefab);
        //Layers and mesh for cursor
        mesh = cursor.GetComponent<MeshRenderer>();
        cursor.layer = 8;
        foreach (Transform child in cursor.transform)
        {
            child.gameObject.layer = 8;
            childMesh = child.GetComponent<MeshRenderer>();
            childMesh.material = placeable;
        }
    }

    public MouseMode GetMouseMode()
    {
        return mouseMode;
    }

    public void SwitchToSelect()
    {
        mouseMode = MouseMode.Select;
        if(cursor!= null)
        {
            Destroy(cursor);
            cursor = null;
        }
    }

    public int GetCursorType()
    {
        if(cursor == null)
        {
            return 0;
        }
        return cursor.GetComponent<BuildingController>().getBldType();
    }


    //Objects necessary for building placement
    private GameObject buildingPrefab;
    public Player player;
    public GameObject cursor;

    private MeshRenderer mesh;
    private MeshRenderer childMesh;
    public Material placeable;
    public Material unplaceable;

    public void SwitchToPlacement(GameObject buildingPrefab)
    {
        mouseMode = MouseMode.Placement;
        //Find the grid, stores information about whether a space is placeable
        //Glowy construct of where the building will be placed
        cursor = Instantiate(buildingPrefab);
        //Layers and mesh for cursor
        mesh = cursor.GetComponent<MeshRenderer>();
        cursor.layer = 8;
        foreach (Transform child in cursor.transform)
        {
            child.gameObject.layer = 8;
            childMesh = child.GetComponent<MeshRenderer>();
            childMesh.material = placeable;
        }
        BuildingController building = buildingPrefab.GetComponent<BuildingController>();
        building.givePlayer(player);
    }
}

public enum MouseMode
{
    NoSelect, Select, Placement
}
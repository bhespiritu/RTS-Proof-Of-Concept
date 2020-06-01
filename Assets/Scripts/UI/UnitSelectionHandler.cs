using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelectionHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    Image selectionImage;
    Vector2 clickStart;
    Rect selectRect;

    List<Selectable> selected = new List<Selectable>();

    public Material selectedMat;
    public Material unselectedMat;

    public LayerMask selectionMask;

    public GameObject selectionPrefab;

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        selectionImage.gameObject.SetActive(true);
        clickStart = eventData.position;
        selectRect = new Rect();
    }

    public void OnDrag(PointerEventData eventData)
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            DeselectAll();
        }

        selectionImage.gameObject.SetActive(false);
        foreach(Selectable s in Selectable.allSelectable)
        {
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(s.transform.position);
            if(selectRect.Contains(screenPos))
            {
                Select(s);
            }
        }
    }

    private RaycastHit clickCast = new RaycastHit();

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            DeselectAll();
        }

        Ray clickWorldRay = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(clickWorldRay, out clickCast))
        {
            Selectable s = clickCast.transform.GetComponent<Selectable>();
            if(s)
            {
                Select(s);
            }
        }
    }

    public void Select(Selectable s)
    {
        Deselect(s);
        selected.Add(s);
        s.GetComponent<Renderer>().material = selectedMat;

        GameObject selectionCircle = Instantiate(selectionPrefab, s.transform);
        selectionCircle.transform.localPosition = Vector3.up * -0.5f;
        s.selectionObject = selectionCircle;
    }


    public void Deselect(Selectable s, bool removeItem = true)
    {
        if(removeItem)
            selected.Remove(s);
        s.GetComponent<Renderer>().material = unselectedMat;
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
}

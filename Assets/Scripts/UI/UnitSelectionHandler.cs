using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelectionHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    Image selectionImage;
    Vector2 clickStart;
    Rect selectRect;

    List<Selectable> selected = new List<Selectable>();

    public Material selectedMat;
    public Material unselectedMat;

    public LayerMask selectionMask; 

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
            foreach (Selectable s in selected)
            {
                s.GetComponent<Renderer>().material = unselectedMat;
            }
            selected.Clear();
        }

        selectionImage.gameObject.SetActive(false);
        foreach(Selectable s in Selectable.allSelectable)
        {
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(s.transform.position);
            if(selectRect.Contains(screenPos))
            {
                selected.Add(s);
                s.GetComponent<Renderer>().material = selectedMat;
            }
        }
    }

}

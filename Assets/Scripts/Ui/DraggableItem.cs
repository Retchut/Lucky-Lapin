    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : EventTrigger
{
    //public Transform dragAreasContainer;
    bool dragging;
    Vector3 initPos;

    private DragArea[] dragAreas;

    private DragArea currentArea;

    private void Start()
    {
        //if (dragAreas == null)
        dragAreas = Transform.FindObjectsOfType<DragArea>();
        //dragAreasContainer.FindObjectOfType<DragArea>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragging) return;

        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        initPos = transform.position;
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        DragArea foundArea = null;

        foreach (var area in dragAreas)
        {
            if (area == null || area.gameObject == null || !area.gameObject.activeSelf || !CheckIfCursorInside(area.rect)) continue;
            foundArea = area;
            break;
        }

        if (foundArea == null || foundArea == currentArea)
            transform.position = initPos;
        else
        {
            foundArea.OnDrag(this);
        }

    }


    public void SetCurrentArea(DragArea area)
    {
        if (currentArea != null)
            currentArea.RemoveObject();

        currentArea = area;
    }


    private static bool CheckIfCursorInside(RectTransform rect)
    {

        Vector2 localMousePosition = rect.InverseTransformPoint(Input.mousePosition);
        return (rect.rect.Contains(localMousePosition));

        //var mousePosition = Input.mousePosition;
        //var normalizedMousePosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        //return (normalizedMousePosition.x > rect.anchorMin.x &&
        //    normalizedMousePosition.x < rect.anchorMax.x &&
        //    normalizedMousePosition.y > rect.anchorMin.y &&
        //    normalizedMousePosition.y < rect.anchorMax.y);

        //return Input.mousePosition.x > rect.rect.xMin && Input.mousePosition.x < rect.rect.xMax &&
        //  Input.mousePosition.y > rect.rect.yMin && Input.mousePosition.y < rect.rect.yMax;
    }
}

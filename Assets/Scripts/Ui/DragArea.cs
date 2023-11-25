using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public abstract class DragArea : MonoBehaviour
{
    public Transform positionPlacer;

    public DraggableItem itemPlaced { get; private set; }
    public RectTransform rect { get; private set; }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public abstract void OnDrag(DraggableItem item, bool switchInput = true);

    public void PlaceInArea(DraggableItem item)
    {
        item.SetCurrentArea(this);
        item.transform.SetParent(transform);
        if (positionPlacer != null)
            item.transform.position = positionPlacer.position;
    }
    public void SetObject(DraggableItem item)
    {
        itemPlaced = item;
    }
    public void RemoveObject()
    {
        itemPlaced = null;
    }

}

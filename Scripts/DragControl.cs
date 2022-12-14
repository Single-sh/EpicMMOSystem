﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace EpicMMOSystem
{
public class DragControl : MonoBehaviour
{
    private RectTransform window;
    //delta drag
    private Vector2 delta;

    public void Awake()
    {
        var window2 = (RectTransform)transform;
        window = EpicMMOSystem.RestorePosition(window2); 
       //window = window2;
    }

    public void BeginDrag()
    {
        delta = Input.mousePosition - window.position;
            EpicMMOSystem.MLLogger.LogWarning("BeginDrag being called");
        }
    public void Drag()
    {
        Vector2 newPos = (Vector2)Input.mousePosition - delta;
        Vector2 Transform = new Vector2(window.rect.width * transform.root.lossyScale.x, window.rect.height * transform.root.lossyScale.y);
        Vector2 OffsetMin, OffsetMax;
        OffsetMin.x = newPos.x - window.pivot.x * Transform.x;
        OffsetMin.y = newPos.y - window.pivot.y * Transform.y;
        OffsetMax.x = newPos.x + (1 - window.pivot.x) * Transform.x;
        OffsetMax.y = newPos.y + (1 - window.pivot.y) * Transform.y;
        if (OffsetMin.x < 0)
        {
            newPos.x = window.pivot.x * Transform.x;
        }
        else if (OffsetMax.x > Screen.width)
        {
            newPos.x = Screen.width - (1 - window.pivot.x) * Transform.x;
        }
        if (OffsetMin.y < 0)
        {
            newPos.y = window.pivot.y * Transform.y;
        }
        else if (OffsetMax.y > Screen.height)
        {
            newPos.y = Screen.height - (1 - window.pivot.y) * Transform.y;
        }
        window.position = newPos;
    }

      public void OnEndDrag()
        {
            EpicMMOSystem.SaveWindowPositions(window.gameObject, false);
        }
    }
}

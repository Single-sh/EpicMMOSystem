using UnityEngine;
using UnityEngine.EventSystems;
namespace EpicMMOSystem;
public class DragMenu : MonoBehaviour, IDragHandler
{
    public Transform menu;

    public void OnDrag(PointerEventData eventData)
    {
        menu.position += (Vector3)eventData.delta;
    }
}
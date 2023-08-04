using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    private float distance;
    private float Distance => body.rect.xMax;
    private RectTransform body;

    private bool onDrag = false;

    private void Awake()
    {
        body = transform.GetChild(0) as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log("asd");
        onDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

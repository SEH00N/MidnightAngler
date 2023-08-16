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

    private void Update()
    {
        //transform.position 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

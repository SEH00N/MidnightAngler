using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    [Tooltip("Speed of return to origin position when inactivated")]
    [SerializeField] float returningSpeed = 10f;

    // return joystick values when its activated, return Vector2.zero when its inactivated
    [Space(20f)]
    [SerializeField] UnityEvent<Vector2> OnValueChanged;
    [SerializeField] UnityEvent OnReleased;

    private RectTransform body;
    private Transform stick;

    private float distance;
    private bool onDrag = false;

    private void Awake()
    {
        body = transform as RectTransform;
        stick = transform.GetChild(0);
    }

    private void Start()
    {
        distance = body.rect.xMax;
    }

    private void Update()
    {
        if(onDrag == false)
        {
            stick.position = Vector3.Lerp(stick.position, body.position, Time.deltaTime * returningSpeed);
            return;
        }

        Vector2 value = Input.mousePosition - body.position;
        value = (value.magnitude < distance) ? value : value.normalized * distance;
        stick.position = body.position + (Vector3)value;

        OnValueChanged?.Invoke(value.normalized);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onDrag = false;
        OnReleased?.Invoke();
    }
}

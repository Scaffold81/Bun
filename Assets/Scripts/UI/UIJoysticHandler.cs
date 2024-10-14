using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class UIJoysticHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject _joystickStick;

    [SerializeField]
    private float _joystickRadius = 200;

    [SerializeField]
    private float _deadZoneRadius = 50; // Радиус слепой зоны

    private Vector3 _direction;
    private Vector2 mousePosition;
    private Vector2 touchPosition;
    private bool onJoystic;

    private InputAction mouseAction;
    private InputAction touchAction;

    public Action<Vector2> Direction { get; set; }

    private void OnEnable()
    {
        mouseAction = new InputAction(binding: "<Mouse>/position");
        mouseAction.performed += ctx => MousePerformed(ctx);
        mouseAction.Enable();

        touchAction = new InputAction(binding: "<Touchscreen>/primaryTouch");

        touchAction.performed += ctx => TouchPerformed(ctx);
        touchAction.canceled += ctx => TouchCanceled();
        touchAction.Enable();
    }

    private void OnDisable()
    {
        mouseAction.Disable();
        touchAction.Disable();
    }
    private void MousePerformed(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();

        float xDirection = Mathf.Abs(mousePosition.x - transform.position.x) > _deadZoneRadius ? mousePosition.x - transform.position.x : 0;
        float yDirection = Mathf.Abs(mousePosition.y - transform.position.y) > _deadZoneRadius ? mousePosition.y - transform.position.y : 0;

        _direction = new Vector2(xDirection, yDirection);

        if (!onJoystic)
        {
            _direction = Vector2.zero;
            return;
        }

        Direction(_direction);
    }

    private void TouchPerformed(InputAction.CallbackContext context)
    {
        TouchControl touch = context.control as TouchControl;
        Vector2 touchPosition = touch.position.ReadValue();
        float xDirection = Mathf.Abs(touchPosition.x - transform.position.x) > _deadZoneRadius ? touchPosition.x - transform.position.x : 0;
        float yDirection = Mathf.Abs(touchPosition.y - transform.position.y) > _deadZoneRadius ? touchPosition.y - transform.position.y : 0;

        _direction = new Vector2(xDirection, yDirection);

        if (!onJoystic)
        {
            _direction = Vector2.zero;
            return;
        }

        Direction(_direction);
    }

    private void TouchCanceled()
    {
        _direction = Vector2.zero;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!onJoystic) return;
        Vector2 clampedDirection = _direction.normalized * Mathf.Clamp(_direction.magnitude, 0, _joystickRadius); // Ограничиваем движение по известному направлению в пределах радиуса
        _joystickStick.transform.position = (Vector2)transform.position + clampedDirection; // Устанавливаем позицию _stick с ограничением по радиусу относительно центра
                                                                              // _stick.transform.position = transform.position + _direction;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onJoystic = true;
        _joystickStick.transform.position = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onJoystic = false;
        _joystickStick.transform.position = transform.position;
        Direction(Vector3.zero);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
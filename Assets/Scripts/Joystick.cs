using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private Image joystickImage = null;

    [SerializeField]
    private RectTransform joystickSlider = null;

    private int dragOffset = 100;
    private bool enablePlayerMovement = false;

    public void OnDrag (PointerEventData eventData)
    {
        Vector2 dragPos = Vector2.negativeInfinity;
        RectTransformUtility.ScreenPointToLocalPointInRectangle (joystickSlider,
                                                            eventData.position,
                                                            null,
                                                            out dragPos);

        dragPos = Vector2.ClampMagnitude ( dragPos, dragOffset ) / dragOffset;
        rotateSlider (eventData.position);
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        joystickSlider.transform.eulerAngles = Vector3.zero;
        joystickImage.CrossFadeAlpha ( 0, 0.5f, false );
        enablePlayerMovement = false;
        EventsManager.StopPlayerMovement?.Invoke ();
    }

    private void rotateSlider (Vector2 drag)
    {
        float x = drag.x - joystickSlider.transform.position.x;
        float y = drag.y - joystickSlider.transform.position.y;
        float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        if ( angle < 0 )
        {
            angle += 360;
        }
        joystickSlider.transform.eulerAngles = new Vector3 ( 0, 0, -angle );
        
        EventsManager.RotatePlayer?.Invoke ( angle );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickImage.CrossFadeAlpha ( 1, 0.3f, false );
        enablePlayerMovement = true;
        StartCoroutine (keepMovingPlayer());
    }

    private IEnumerator keepMovingPlayer ()
    {
        while ( enablePlayerMovement )
        {
            EventsManager.StartPlayerMovement?.Invoke();

            yield return new WaitForEndOfFrame ();
        }
    }
}

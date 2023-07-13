using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIElementXR : MonoBehaviour
{

    public UnityEvent OnXRPointerEnter;
    public UnityEvent OnXRPointerExit;

    private Camera xRCamera;

    
    void Start()
    {
        xRCamera = CameraPointerManager.Instance.gameObject.GetComponent<Camera>();
    }

    // Es crean les funcions per a hi ha que cridar als botons. Es com si es fa un click al botó

    public void OnPointerClickXR()
    {
        PointerEventData pointerEvent = PlacePointer(); // Tenim el pointer sobre el elment de la UI
        //S'executa l'acció
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerEnterXR()
    {
        GazeManager.Instance.SetUpGaze(1.8f); // Un temps de selecció de 2 segons
        OnXRPointerEnter?.Invoke();  // Es crida a l'event que s'ha creat
        PointerEventData pointerEvent = PlacePointer(); // Tenim el pointer sobre el elment de la UI
        //S'executa l'acció
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerExitXR()
    {
        GazeManager.Instance.SetUpGaze(2.5f);
        OnXRPointerExit?.Invoke();
        PointerEventData pointerEvent = PlacePointer(); // Tenim el pointer sobre el elment de la UI
        //S'executa l'acció
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerUpHandler);
    }

    private PointerEventData PlacePointer()
    {
        Vector3 screenPos = xRCamera.WorldToScreenPoint(CameraPointerManager.Instance.hitPoint);
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = new Vector2(screenPos.x, screenPos.y);
        return pointer;
    }
}

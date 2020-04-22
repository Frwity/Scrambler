using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputModule : PointerInputModule
{

   
    
    public string ClickInputName = "Submit";
    public RaycastResult CurrentRaycast;
     
    private PointerEventData pointerEventData;
    private GameObject currentLookAtHandler;

   
    public override void Process()
    {
        SetPointerPosition();
        HandleRaycast();
        HandleSelection();
        
        bool releasing = Input.GetButtonUp(ClickInputName) || Input.GetMouseButtonUp(0);
        bool clicking = Input.GetButtonDown(ClickInputName) || Input.GetMouseButtonDown(0);
        ProcessTouchPress(pointerEventData, clicking, releasing);
    }
     
    private void SetPointerPosition()
    {
        if (pointerEventData == null)
        { 
            pointerEventData = new PointerEventData(eventSystem);
        }

        
       
        pointerEventData.position =GetComponent<pointer>().pos;

        
        pointerEventData.delta = Vector2.zero;
        
    }
     
     private void HandleRaycast()
     {
         List<RaycastResult> raycastResults = new List<RaycastResult>();
         eventSystem.RaycastAll(pointerEventData, raycastResults);
         CurrentRaycast = pointerEventData.pointerCurrentRaycast = FindFirstRaycast(raycastResults);
     
         ProcessMove(pointerEventData);
     }
     
     private void HandleSelection()
     {
         if (pointerEventData.pointerEnter != null)
         {
             GameObject handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(pointerEventData.pointerEnter);
     
             if (currentLookAtHandler != handler)
             {
                currentLookAtHandler = handler;
             }
     
             if (currentLookAtHandler != null && Input.GetMouseButtonDown(0))
             {
                ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, pointerEventData, ExecuteEvents.pointerClickHandler);
             }
         }
         else
         {
             currentLookAtHandler = null;
         }
     }
     
     private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
        {
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;
            DeselectIfSelectionChanged(currentOverGo, pointerEvent);
            // PointerDown notification
            if (pressed)
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
 
                if (pointerEvent.pointerEnter != currentOverGo)
                {
                    // send a pointer enter to the touched element if it isn't the one to select...
                    HandlePointerExitAndEnter(pointerEvent, currentOverGo);
                    pointerEvent.pointerEnter = currentOverGo;
                }
 
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);
               
                if (newPressed == null)
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
               
                float time = Time.unscaledTime;
               
                if (newPressed == pointerEvent.lastPress)
                {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;
                   
                    pointerEvent.clickTime = time;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }
               
                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;
               
                pointerEvent.clickTime = time;
               
                // Save the drag handler as well
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);
               
                if (pointerEvent.pointerDrag != null)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
            }
           
            // PointerUp notification
            if (released)
            {
                // Debug.Log("Executing pressup on: " + pointer.pointerPress);
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
 
                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
               
                // PointerClick and Drop events
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEvent.pointerDrag != null)
                {
                    ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
                }
               
                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;
               
                if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
               
                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;
               
                if (pointerEvent.pointerDrag != null)
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
               
                pointerEvent.pointerDrag = null;
                ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
                pointerEvent.pointerEnter = null;
            }
        }
    

}

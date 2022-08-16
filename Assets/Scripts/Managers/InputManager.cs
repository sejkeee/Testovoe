using System.Collections;
using System.Collections.Generic;
using MetaScene;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private UnityEngine.InputSystem.EnhancedTouch.Touch currentTouch;
        
        public  delegate void MovingInformer(Vector2 currentPos, Vector2 deltaVector);
        public static MovingInformer SingleTouchingStarted;
        public static MovingInformer SingleTouchingMoving;
        public static MovingInformer SingleTouchingEnded;

        private Vector2 startPos;
        private const float DISTANCE_FOR_MOVING = 25;
        
        private void Awake()
        {
            EnhancedTouchSupport.Enable();
        }

        private void Update()
        {
            if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 1)
            {
                currentTouch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
                
                if(UICallback.isOverUI(currentTouch.screenPosition))
                    return;

                if(currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    SingleTouchingStarted?.Invoke(currentTouch.screenPosition, currentTouch.delta);
                    startPos = currentTouch.screenPosition;
                }
                else if (currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Canceled || currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                {
                    SingleTouchingEnded?.Invoke(currentTouch.screenPosition, currentTouch.delta);
                }
                else if (Vector2.Distance(startPos, currentTouch.screenPosition) > DISTANCE_FOR_MOVING)
                {
                    SingleTouchingMoving?.Invoke(currentTouch.screenPosition, currentTouch.delta);
                }
            }
        }
    }
}
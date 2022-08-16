using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MetaScene
{
    public class UICallback : MonoBehaviour
    {
        private static GraphicRaycaster G_Raycaster;
        private static PointerEventData pointerEvent;
        private static EventSystem EventSys;

        private void Start()
        {
            EventSys = EventSystem.current;
            G_Raycaster = GetComponent<GraphicRaycaster>();
            pointerEvent = new PointerEventData(EventSys);
        }

        public static bool isOverUI(Vector2 touchPos)
        {
            pointerEvent.position = touchPos;

            List<RaycastResult> results = new List<RaycastResult>();
            G_Raycaster.Raycast(pointerEvent, results);
            if (results.Count != 0)
                return true;
            return false;
        }
    }
}

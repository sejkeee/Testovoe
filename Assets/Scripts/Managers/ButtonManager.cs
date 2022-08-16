using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class ButtonManager : MonoBehaviour
    {
        public static UnityEvent StartPressed = new UnityEvent();
        public static UnityEvent<int> SwapCannon = new UnityEvent<int>();
        
        public void PressStart() => StartPressed?.Invoke();
        public void PressSwap(int i) => SwapCannon?.Invoke(i);
    }
}
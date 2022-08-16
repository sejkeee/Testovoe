using Abstracts;
using Managers;
using UnityEngine.Events;

namespace Game
{
    public class StartGame : IGameState
    {
        public static UnityEvent ShowStart = new UnityEvent();
        public static UnityEvent CloseStart = new UnityEvent();
        public MainManager MainManager { get; set; }

        public void Enter()
        {
            ShowStart?.Invoke();
        }

        public void Exit()
        {
            CloseStart?.Invoke();
        }
    }
}
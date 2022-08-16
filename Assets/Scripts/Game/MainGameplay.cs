using Abstracts;
using Managers;
using UnityEngine;

namespace Game
{
    public class MainGameplay : IGameState
    {
        private ICannon activeCannon;
        public MainManager MainManager { get; set; }

        public void SetActiveCannon(ICannon cannon)
        {
            if(activeCannon != null)
            {
                InputManager.SingleTouchingStarted -= activeCannon.Targeting;
                InputManager.SingleTouchingMoving -= activeCannon.Targeting;
                InputManager.SingleTouchingEnded -= activeCannon.StopTargeting;
            }
            
            activeCannon?.Stop();
            activeCannon = cannon;
            activeCannon.Start();
            
            InputManager.SingleTouchingStarted += activeCannon.Targeting;
            InputManager.SingleTouchingMoving += activeCannon.Targeting;
            InputManager.SingleTouchingEnded += activeCannon.StopTargeting;
        }

        public void Enter()
        {
            MainManager.ShipCreator.Init(5f);
        }

        public void Exit()
        {
            MainManager.ShipCreator.Deinit();
        }
    }
}
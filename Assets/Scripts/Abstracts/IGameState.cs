using Managers;

namespace Abstracts
{
    public interface IGameState
    {
        public MainManager MainManager { get; set; }
        public void Enter();
        public void Exit();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Abstracts;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public enum GameState
    {
        StartGame,
        MainGameplay
    }
    
    public class MainManager : MonoBehaviour
    {
        public static UnityEvent<int> ScoreChanged = new UnityEvent<int>();

        private Dictionary<GameState, IGameState> GameStatesMap = new Dictionary<GameState, IGameState>();
        [SerializeField] private CannonObject[] _cannonObjects;
        private Dictionary<ICannon, CannonObject> CanonsMap = new Dictionary<ICannon, CannonObject>();

        private IGameState currentState;

        private Cannon _cannon = new Cannon();
        private MachineGun _machineGun = new MachineGun();
        private RocketLauncher _rocketLauncher = new RocketLauncher();

        private List<ICannon> Cannons = new List<ICannon>();
        private CannonObject currentCannonObject;

        private int currentCannonIndex = 0;

        private Camera MainCamera;

        [field: SerializeField]
        public ShipCreator ShipCreator{get; private set; }

        public int Score { get; set; }
        
        private void Awake()
        {
            GameStatesMap.Add(GameState.StartGame, new StartGame());
            GameStatesMap.Add(GameState.MainGameplay, new MainGameplay());

            foreach (var state in GameStatesMap)
            {
                state.Value.MainManager = this;
            }
            
            CanonsMap.Add(_cannon, _cannonObjects[0]);
            CanonsMap.Add(_machineGun, _cannonObjects[1]);
            CanonsMap.Add(_rocketLauncher, _cannonObjects[2]);
            
            Cannons.Add(_cannon);
            Cannons.Add(_machineGun);
            Cannons.Add(_rocketLauncher);

            _cannon.MainManager = this;
            _machineGun.MainManager = this;
            _rocketLauncher.MainManager = this;
            
            ButtonManager.StartPressed?.AddListener(StartGame);
            ButtonManager.SwapCannon?.AddListener(SwapCannon);
            ShipAbstract.Destroyed?.AddListener(ship =>
            {
                Score++;
                ScoreChanged?.Invoke(Score);
            });
            
            MainCamera = Camera.main;
        }

        private void Start()
        {
            SetStateAtStart();
        }

        private void StartGame()
        {
            (GameStatesMap[GameState.MainGameplay] as MainGameplay)?.SetActiveCannon(_cannon);
            SetState(GameState.MainGameplay);
        }

        #region States

        private void SetStateAtStart()
        {
            SetState(GameState.StartGame);
        }
        
        private void SetState(GameState state)
        {
            currentState?.Exit();

            currentState = GameStatesMap[state];
            currentState.Enter();
        }

        #endregion

        #region Cannons

        public void StartCannon(ICannon sender)
        {
            currentCannonObject = CanonsMap[sender];
            CanonsMap[sender].Init(currentCannonIndex);
        }
        public void RotateCannon(Vector2 delta, ICannon sender)
        {
            var cannonObject = CanonsMap[sender];
            cannonObject.Rotate(delta);
        }
        public void StopTargeting(ICannon sender)
        {
            var cannonObject = CanonsMap[sender];
            cannonObject.StopRendering();
        }
        public void ShotCannon(ICannon sender)
        {
            var cannonObject = CanonsMap[sender];
            cannonObject.Shot((int)sender.Damage);
        }

        public void ShotMachineGun(ICannon sender)
        {
            var cannonObject = CanonsMap[sender];
            Ray ray = new Ray(cannonObject.transform.position, cannonObject.transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag == "Ship")
                {
                    currentCannonObject.Shot(Vector3.Distance(cannonObject.transform.position
                        , hit.transform.position));
                    
                    hit.transform.GetComponent<ShipAbstract>().TakeDamage(Cannons[1].Damage);
                }
            }
            else
            {
                currentCannonObject.Shot(Vector3.Distance(cannonObject.transform.position, cannonObject.transform.forward * 1000));
            }
        }
        
        public void TargetRocket(Vector2 pos)
        {
            if (Physics.Raycast(MainCamera.ScreenPointToRay(pos), out RaycastHit hit))
            {
                if (hit.transform.tag == "Ship")
                {
                    currentCannonObject.TargetRocket(hit.transform.gameObject, Cannons[0].Damage);
                    StartCoroutine(ActionAfterTime(
                        ()=> hit.transform.GetComponent<ShipAbstract>().TakeDamage(Cannons[2].Damage), 3f));
                }
            }
        }

        private IEnumerator ActionAfterTime(Action action, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            action?.Invoke();
        }
        
        private void SwapCannon(int to)
        {
            try
            {
                currentCannonObject.Deinit();
                currentCannonIndex += to;
                (GameStatesMap[GameState.MainGameplay] as MainGameplay)?.SetActiveCannon(Cannons[currentCannonIndex]);
                currentCannonObject = CanonsMap[Cannons[currentCannonIndex]];
                currentCannonObject.Init(currentCannonIndex);
            }
            catch
            { Debug.LogWarning("No cannon"); }
            
        }
        #endregion
        
    }
}
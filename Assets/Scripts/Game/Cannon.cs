using System.Threading.Tasks;
using Abstracts;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Cannon : ICannon
    {
        public MainManager MainManager { get; set; }
        public float ReloadTime => 3f;
        public float CurrentReloading { get; set; }

        public bool AbleShot => CurrentReloading >= ReloadTime;

        public void Start()
        {
            CurrentReloading = ReloadTime;
            MainManager.StartCannon(this);
        }

        public void Targeting(Vector2 pos, Vector2 delta)
        {
            MainManager.RotateCannon(delta, this);
        }

        public void StopTargeting(Vector2 pos, Vector2 delta)
        {
            MainManager.StopTargeting(this);
            Shot(pos, delta);
        }

        public void Shot(Vector2 pos, Vector2 delta)
        {
            if(!AbleShot)
                return;
            
            MainManager.ShotCannon(this);
            Reloading();
        }

        public void Reloading()
        {
            Reload();
        }

        private async void Reload()
        {
            CurrentReloading = 0f;
            while (CurrentReloading < ReloadTime)
            {
                await Task.Delay(100);
                CurrentReloading += .1f;
            }
        }

        public void Stop()
        {
            
        }

        public float Damage => 10;
    }
}
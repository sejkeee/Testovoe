using System.Threading.Tasks;
using Abstracts;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class MachineGun : ICannon
    {
        public MainManager MainManager { get; set; }
        public float ReloadTime => .1f;
        public float CurrentReloading { get; set; }

        private bool isTargeting = false;

        private Vector2 position;
        private Vector2 delta;

        public bool AbleShot => CurrentReloading >= ReloadTime;

        public void Start()
        {
            CurrentReloading = ReloadTime;
            MainManager.StartCannon(this);
        }

        public void Targeting(Vector2 pos, Vector2 delta)
        {
            MainManager.RotateCannon(delta, this);
            
            this.delta = delta;
            this.position = pos;
            
            isTargeting = true;
            
            MachineShooting();
        }

        private async void MachineShooting()
        {
            while (isTargeting)
            {
                Shot(delta,position);
                await Task.Yield();
            }
        }

        public void StopTargeting(Vector2 pos, Vector2 delta)
        {
            MainManager.StopTargeting(this);
            isTargeting = false;
        }

        public void Shot(Vector2 pos, Vector2 delta)
        {
            if(!AbleShot)
                return;
            
            MainManager.ShotMachineGun(this);
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
            isTargeting = false;
        }

        public float Damage => 1;
    }
}
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

namespace Abstracts
{
    public interface ICannon
    {
        public MainManager MainManager { get; set; }
        public float ReloadTime { get; }
        public float CurrentReloading { get; set; }
        public bool AbleShot { get; }

        public void Start();
        public void Targeting(Vector2 pos, Vector2 delta);
        public void StopTargeting(Vector2 pos, Vector2 delta);
        public void Shot(Vector2 pos, Vector2 delta);
        public void Reloading();
        public void Stop();

        public float Damage { get; }
    }
}
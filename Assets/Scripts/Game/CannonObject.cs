using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum CannonType
    {
        Cannon = 20,
        MachineGun = 500,
        RocketLauncher
    }
    public class CannonObject : MonoBehaviour
    {
        [Header("Cannons")] [SerializeField] private CannonType cannonType;
        
        [SerializeField] private TrajectoryRenderer trajectory;
        
        private const float rotateSpeed = .1f;
        private Vector3 eulerAngles;

        [SerializeField] private Projectile projectile;
        [SerializeField] private Transform shotPoint;
        private Vector3 speed;

        private Dictionary<int, CannonType> typeMap =
            new Dictionary<int, CannonType>()
            {
                {0, CannonType.Cannon},
                {1, CannonType.MachineGun},
                {2, CannonType.MachineGun}
            };

        public void Init(int type)
        {
            gameObject.SetActive(true);
            eulerAngles = transform.localEulerAngles;
            cannonType = typeMap[type];
        }

        public void Rotate(Vector3 eulerAngels)
        {
            transform.localEulerAngles += new Vector3(eulerAngels.y, -eulerAngels.x)*rotateSpeed;
            
            speed = transform.forward * (int)cannonType;
            
            trajectory.DrawTrajectory(transform.position, speed);
        }

        public void Shot(int damage)
        {
            if (speed == Vector3.zero)
                return;
            
            var shotProjectile = Instantiate(projectile, shotPoint.position, transform.rotation).GetComponent<Rigidbody>();
            shotProjectile.AddForce(speed, ForceMode.VelocityChange);
            shotProjectile.GetComponent<Projectile>().Init(damage);
            StopRendering();
        }
        
        public void Shot(float distance)
        {
            if (speed == Vector3.zero)
                return;
            
            var shotProjectile = Instantiate(projectile, shotPoint.position, transform.rotation).GetComponent<Rigidbody>();
            shotProjectile.AddForce(speed, ForceMode.VelocityChange);
            StartCoroutine(DestroyMGBullet(distance, shotProjectile));
        }

        private IEnumerator DestroyMGBullet(float distance, Rigidbody proj)
        {
            yield return new WaitForSecondsRealtime(distance / 500);
            try
            {
                Destroy(proj.gameObject);
            }
            catch
            {
            }
        }
        
        public void StopRendering() => trajectory.StopRendering();

        public void TargetRocket(GameObject target, float damage)
        {
            var shotProjectile = Instantiate(projectile, shotPoint.position, transform.rotation);
            shotProjectile.Init(target, 50f, damage);
        }

        public void Deinit()
        {
            gameObject.SetActive(false);
        }
    }
}
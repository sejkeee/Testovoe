using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private float speed;
        private GameObject Target;
        private bool MaylookAt;
        private float damage;
        public void Init(GameObject target, float speed, float damage)
        {
            this.speed = speed;
            Target = target;
            this.damage = damage;

            var pos = transform.position;
            StartCoroutine(Coroutines.LerpGameObjectFromTo(transform, pos + transform.forward * 5, 1f, EasingFunction.Linear));
            StartCoroutine(Coroutines.LerpGameObjectFromTo(transform, Target.transform.position, 1f, EasingFunction.Linear, timeToWait: 1f, false));
        }
        
        public void Init(float damage)
        {
            this.damage = damage;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            MaylookAt = true;
            yield return new WaitForSecondsRealtime(4);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if(!Target || !MaylookAt)
                return;
            
            transform.LookAt(Target.transform);
        }

        private void OnCollisionEnter(Collision collision)
        {
            collision.gameObject.TryGetComponent<ShipAbstract>(out ShipAbstract ship);
            Debug.LogWarning(damage);
            if(!ship) 
                return;
            
            ship.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ShipAbstract : MonoBehaviour
{
    private float speed;
    [SerializeField] private float health;

    public static UnityEvent<ShipAbstract> Destroyed = new UnityEvent<ShipAbstract>();

    public virtual void Init(float speed, float health, Quaternion rotation)
    {
        this.speed = speed;
        this.health = health;

        transform.rotation = rotation;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(40);
        Destroy(gameObject);
    }

    private void Update()
    {
        Move();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if(health <= 0)
            Die();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedTime);
    }

    protected virtual void Die()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }
}

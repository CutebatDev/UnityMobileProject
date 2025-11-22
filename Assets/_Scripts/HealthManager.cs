using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts
{
    public class HealthManager : MonoBehaviour
    {
        public float currentHealth = 10; // PUBLIC SO IT CAN BE SET TO SCRIPT OBJECT VALUE
        private bool _isPlayer = false;

        private void Start()
        {
            _isPlayer = CompareTag("Player");
            if (!gameObject.TryGetComponent<Collider>(out var component))
            {
                Debug.Log("Collider on " + gameObject.name + " not found.");
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if(_isPlayer)
            {
                if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyProjectile"))
                {
                    TakeDamage(1);
                }
            }
            else if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerProjectile"))
            {
                // maybe enemies dont take contact damage from player?
                TakeDamage(1);
            }
        }
        private void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) 
                ObjectDeath();
        }

        private void ObjectDeath()
        {
            if (_isPlayer)
            {
                
                // do gameover things
            }
            else
            {
                // do enemy death things
                // PoolManager.Instance.ReturnToPool(gameObject.getParent()); idk smth like this
                // also ScoreManager.Instance.AddScore(100500)
                // maybe instead lots of instances we can use GameManagers GameObject and just do GameManagers.GetComponent<ScoreManager>().... etc
            }
        }
    }
}
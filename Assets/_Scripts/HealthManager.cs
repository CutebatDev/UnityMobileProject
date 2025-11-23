using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts
{
    public class HealthManager : MonoBehaviour
    {
        public float maxHealth = 10;
        public float currentHealth = 10; // PUBLIC SO IT CAN BE SET TO SCRIPT OBJECT VALUE
        private bool _isPlayer = false;

        #region IFrames parametes

        private bool _isInvulnerable = false;
        private float _InvulnerablilityDuration = 0.5f;
        private float _iTimer = 0;

        #endregion

        private void Start()
        {
            _isPlayer = CompareTag("Player");
            if (!gameObject.TryGetComponent<Collider>(out var component))
            {
                Debug.Log("Collider on " + gameObject.name + " not found.");
            }
        }

        private void FixedUpdate()
        {
            if(Time.time > _iTimer) _isInvulnerable = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if(_isPlayer)
            {
                if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyProjectile"))
                {
                    if(!_isInvulnerable)
                    {
                        TakeDamage(1);
                        _isInvulnerable = true;
                        _iTimer = Time.time + _InvulnerablilityDuration;
                    }
                }
            }
            else if (other.gameObject.CompareTag("PlayerProjectile") || other.gameObject.CompareTag("Player")) // remove player collision later
            {
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

                Time.timeScale = 0;
            }
            else
            {
                ScoreManager.Instance.AddScore(10);
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }
    }
}
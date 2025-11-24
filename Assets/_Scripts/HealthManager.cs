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
            else if (other.gameObject.CompareTag("PlayerProjectile"))
            {
                if (other.TryGetComponent(out PlayerProjectileController component))
                {
                    TakeDamage(component.bulletDamage);
                }
            }
        }
        private void TakeDamage(float damage)
        {
            if(_isPlayer)
                AudioManager.Instance.PlaySFX(SFX.Player_Hurt);
            currentHealth -= damage;
            if (currentHealth <= 0) 
                ObjectDeath();
        }

        private void ObjectDeath()
        {
            if (_isPlayer)
            {
                UIHandler.Instance.GameOver();
            }
            else
            {
                AudioManager.Instance.PlaySFX(SFX.Enemy_Death);
                gameObject.TryGetComponent(out Enemy enemy);
                ScoreManager.Instance.AddScore(enemy.scoreReward);
                ExpManager.Instance.AddExp(enemy.scoreReward);
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }
    }
}
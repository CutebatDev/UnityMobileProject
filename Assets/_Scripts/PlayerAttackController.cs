using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts
{
    public class PlayerAttackController : MonoBehaviour
    {
        public bool autoShootEnabled = true;
        public GameArea gameArea;
        public int layerIndex;
        public GameObject bulletPrefab;
        public SO_BulletPreset bulletPreset;

        private float nextShotTime = 0;

        public float attackSpeed = 0.5f;
        // TODO cooldowns on differentWeapons
        
        [SerializeField] private PlayerAttackRadius attackArea;
        [SerializeField] public float attackRange = 7;

        private PoolManager poolManager;

        private PoolManager GetPoolManager()
        {
            if(!poolManager)
                poolManager = PoolManager.Instance;
            return poolManager;
        }

        private void Update()
        {
            Transform closestEnemy = GetClosestEnemy();
            if (!closestEnemy)
                autoShootEnabled = false;
            else
                autoShootEnabled = true;
            
            if(autoShootEnabled)
            {
                if (nextShotTime < Time.time)
                {
                    InitializeBullet(transform.position, GetClosestEnemy(), gameArea, layerIndex);
                    nextShotTime = Time.time + attackSpeed;
                }
            }
        }

        private void InitializeBullet(Vector3 spawnPos, Transform target,GameArea gameArea, int layerIndex)
        {
            GameObject bullet = GetPoolManager().GetFromPool(bulletPrefab);
            PlayerProjectileController bulletComponent = bullet.GetComponent<PlayerProjectileController>();
            
            bulletComponent.bulletPreset = bulletPreset;
            bulletComponent.UpdateToPreset();

            bullet.transform.position = transform.position;
            bulletComponent.direction = (target.position - transform.position).normalized;
            bulletComponent.Initialize(gameArea, layerIndex);
        }

        [CanBeNull]
        public Transform GetClosestEnemy()
        {
            Collider closest = null;
            float distanceToClosest = float.MaxValue;
            foreach (var collider in attackArea.enemiesInRange)
            {
                if (closest == null)
                {
                    closest = collider;
                    distanceToClosest = Vector3.Distance(closest.transform.position, gameObject.transform.position);
                }

                float distanceToCurrent = Vector3.Distance(collider.transform.position, gameObject.transform.position);
                if (closest != collider && distanceToCurrent < distanceToClosest)
                {
                    closest = collider;
                    distanceToClosest = distanceToCurrent;
                }
            }
            if(closest != null && distanceToClosest < attackRange) return closest.transform;
            return null;
        }
    }
}
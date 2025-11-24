using System;
using UnityEngine;

namespace _Scripts
{
    public class PlayerProjectileController : MonoBehaviour
    {
        public int bulletDamage = 1;
        public float bulletSpeed = 1;

        public SO_BulletPreset bulletPreset;

        [HideInInspector] public Vector3 direction;
        
        private GameArea _gameArea;
        private int _layerIndex;

        private void Update()
        {
            direction.y = 0; // keep y axis unchanged
            transform.Translate(direction * (bulletSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Enemy"))
                PoolManager.Instance.ReturnToPool(gameObject);
        }
        void FixedUpdate()
        {
            if (_gameArea &&
                _gameArea.IsOutOfBounds(transform.position, _gameArea.layers[_layerIndex]))
            {
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }

        public void UpdateToPreset()
        {
            if(bulletPreset)
            {
                bulletDamage = bulletPreset.bulletDamage;
                bulletSpeed = bulletPreset.bulletSpeed;
            }
        }
        
        public void Initialize(GameArea gameArea, int layerIndex)
        {
            _gameArea = gameArea;
            _layerIndex = layerIndex;
        }
    }
}

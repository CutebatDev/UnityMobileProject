using System;
using UnityEngine;

namespace _Scripts
{
    public class Collectable : MonoBehaviour
    {
        public int value = 1;
        
        private Collider _collider;
        private GameArea _gameArea;
        private int _layerIndex;

        public SO_ExpPreset expPreset;

        public void UpdateToPreset()
        {
            if (expPreset)
            {
                value = expPreset.value;
                transform.localScale = Vector3.one * expPreset.scaleModifier;
            }
        }

        private void Awake()
        {
            _collider = gameObject.GetComponent<Collider>();
            UpdateToPreset();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ExpManager.Instance.AddExp(value);
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }
        
        public void Initialize(SpawnManager owner, GameArea gameArea, int layerIndex)
        {
            _gameArea = gameArea;
            _layerIndex = layerIndex;
        }
        
        void FixedUpdate()
        {
            if (_gameArea != null &&
                _gameArea.IsOutOfBounds(transform.position, _gameArea.layers[_layerIndex]))
            {
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }
    }
}
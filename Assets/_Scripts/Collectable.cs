using System;
using UnityEngine;

namespace _Scripts
{
    public class Collectable : MonoBehaviour
    {
        public int value = 1;
        
        private Collider _collider;

        private void Awake()
        {
            _collider = gameObject.GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // add logics here
            }
        }
        
        
        /* REFERENCE
        public void Initialize(SpawnManager owner, GameArea gameArea, int layerIndex)
        {
            _gameArea = gameArea;
            _layerIndex = layerIndex;
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
         */
    }
}
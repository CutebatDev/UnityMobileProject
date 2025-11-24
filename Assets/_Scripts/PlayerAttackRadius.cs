using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    public class PlayerAttackRadius : MonoBehaviour
    {
        [HideInInspector]
        public List<Collider> enemiesInRange = new List<Collider>();

        private void Update()
        {
            foreach (var VARIABLE in enemiesInRange.ToList())
            {
                if (VARIABLE.enabled == false)
                    enemiesInRange.Remove(VARIABLE);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy") && !enemiesInRange.Contains(other))
            {
                enemiesInRange.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (enemiesInRange.Contains(other))
            {
                enemiesInRange.Remove(other);
            }
        }
        
    }
}
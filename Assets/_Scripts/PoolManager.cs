using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private SpawnManager[] spawnManagers;
        [SerializeField] private bool isPreWarm;
        [SerializeField] private int initialAmount = 1;

        private GameObject[] _prefabs;
        private readonly Dictionary<GameObject, List<GameObject>> _pools = new();
        private readonly Dictionary<GameObject, List<GameObject>> _inactivePools = new();

        private void Awake()
        {
            _prefabs = spawnManagers.SelectMany(sm => sm.GetPrefab()).Distinct().ToArray();

            foreach (var prefab in _prefabs)
            {
                _pools[prefab] = new List<GameObject>();

                if (!isPreWarm) continue;

                for (int i = 0; i < initialAmount; i++)
                {
                    CreateNewObjectInPool(prefab);
                }
            }
        }

        public GameObject GetFromPool(GameObject prefab, SpawnManager owner, GameArea area, int layerIndex)
        {
            if (!_pools.ContainsKey(prefab))
            {
                _pools[prefab] = new List<GameObject>();
            }

            var obj = _pools[prefab].FirstOrDefault(p => !p.activeInHierarchy);

            if (obj == null)
            {
                obj = CreateNewObjectInPool(prefab);
            }

            obj.SetActive(true);
            foreach (var init in obj.GetComponentsInChildren<IPoolSpawnInit>(true))
            {
                init.OnSpawned(owner, area, layerIndex);
            }

            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (obj == null)
                return;
            obj.SetActive(false);

            foreach (var prefab in _inactivePools.Keys)
            {
                if (obj.name.Contains(prefab.name))
                {
                    _inactivePools[prefab].Add(obj);
                    return;
                }
            }
        }

        public GameObject SpawnFromSave(string enemyType, Vector3 position)
        {
            // Try exact match first
            GameObject prefab = _prefabs.FirstOrDefault(p => p.name == enemyType);

            // Fallback: Check if names contain each other (handles "Zombie" vs "ZombieEnemy")
            if (prefab == null)
            {
                prefab = _prefabs.FirstOrDefault(p => p.name.Contains(enemyType) || enemyType.Contains(p.name));
            }

            if (prefab == null)
            {
                Debug.LogWarning(
                    $"[PoolManager] Could not find prefab for type: '{enemyType}'. Available: {string.Join(", ", _prefabs.Select(p => p.name))}");
                return null;
            }

            if (!_pools.ContainsKey(prefab))
            {
                _pools[prefab] = new List<GameObject>();
            }

            var obj = _pools[prefab].FirstOrDefault(p => !p.activeInHierarchy);

            if (obj == null)
            {
                obj = CreateNewObjectInPool(prefab);
            }

            obj.transform.position = position;
            obj.SetActive(true);
            return obj;
        }

        public void DeactivateAllActiveObjects()
        {
            foreach (var poolList in _pools.Values)
            {
                foreach (var obj in poolList)
                {
                    if (obj != null && obj.activeInHierarchy)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }

        private GameObject CreateNewObjectInPool(GameObject prefab)
        {
            var obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            _pools[prefab].Add(obj);
            return obj;
        }
    }
}
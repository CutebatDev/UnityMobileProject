using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    public class PoolManager : MonoBehaviour
    {
        private GameObject[] prefabs;
        [SerializeField] SpawnManager[] spawnManagers;
        private Dictionary<GameObject, List<GameObject>> _pools = new();
        [SerializeField] private bool isPreWarm;
        [SerializeField] private int initialAmount = 1;

        void Awake()
        {
            prefabs = spawnManagers.SelectMany(sm => sm.GetPrefab()).Distinct().ToArray();
            foreach (var prefab in prefabs)
            {
                _pools[prefab] = new List<GameObject>();
                if (isPreWarm)
                {
                    for (int i = 0; i < initialAmount; i++)
                    {
                        var obj = Instantiate(prefab, transform);
                        obj.SetActive(false);
                        _pools[prefab].Add(obj);
                    }
                }
            }
        }

        public GameObject GetFromPool(GameObject prefab, SpawnManager owner, GameArea area, int layerIndex)
        {
            GameObject obj = null;
            foreach (var objectInPool in _pools[prefab])
            {
                if (!objectInPool.activeInHierarchy)
                {
                    obj = objectInPool;
                    break;
                }
            }

            if (obj == null)
            {
                obj = Instantiate(prefab, transform);
                _pools[prefab].Add(obj);
            }

            obj.SetActive(true);
            foreach (var init in obj.GetComponentsInChildren<IPoolSpawnInit>(true))
                init.OnSpawned(owner, area, layerIndex);
            return obj;
        }

        public GameObject ReturnToPool(GameObject prefab, SpawnManager owner, GameArea area, int layerIndex)
        {
            
        }
    }
}
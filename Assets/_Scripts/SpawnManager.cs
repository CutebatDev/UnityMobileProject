using System.Collections;
using UnityEngine;

namespace _Scripts
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] public GameObject[] prefabs;
        [SerializeField] GameArea gameArea;
        [SerializeField] private PoolManager poolManager;
        [SerializeField] private float spawnTimer = 5f;
        public int layerIndex;
        public SO_EnemyPreset[] enemyPresets;
        public LevelParameters difficulty;

        void Start()
        {
            StartCoroutine(SpawnPrefabs());
        }

        IEnumerator SpawnPrefabs()
        {
            while (true)
            {
                var layer = gameArea.layers[layerIndex];

                foreach (var prefab in prefabs)
                {
                    Vector3 spawnPos = gameArea.GetSpawnPosition(layer);
                    var obj = poolManager.GetFromPool(prefab, this, gameArea, layerIndex);

                    int rng = Random.Range(0, enemyPresets.Length);
                    obj.GetComponent<Enemy>().enemyPreset = enemyPresets[rng];
                    obj.GetComponent<Enemy>().UpdateToPreset();
                    obj.transform.position = spawnPos;

                    if (obj.TryGetComponent(out Enemy mover))
                        mover.Initialize(this, gameArea, layerIndex);
                }

                yield return new WaitForSeconds(spawnTimer);
            }
        }


        public GameObject[] GetPrefab()
        {
            return prefabs;
        }
    }
}
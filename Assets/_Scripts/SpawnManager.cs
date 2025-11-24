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
        public LevelParameters levelParameters;

        public static SpawnManager Instance;
        
        void Start()
        {
            Instance = this;
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
                    var obj = poolManager.GetFromPool(prefab);
                    
                    if (obj.TryGetComponent(out Enemy mover))
                        InitializeEnemy(mover, spawnPos);
                    
                    else if (obj.TryGetComponent(out Collectable collectable))
                    {
                         if(collectable.gameObject.CompareTag("CollectableExp"))
                             InitializeExp(collectable, spawnPos);
                    }

                }

                yield return new WaitForSeconds(spawnTimer);
            }
        }

        private void InitializeEnemy(Enemy enemy, Vector3 spawnPos)
        {
            int rng = Random.Range(0, levelParameters.enemyPresets.Length);
            enemy.enemyPreset = levelParameters.enemyPresets[rng];
            enemy.difficulty = levelParameters;
            enemy.UpdateToPreset();
            enemy.transform.position = spawnPos;
            
            enemy.Initialize(this, gameArea, layerIndex);
        }
        
        private void InitializeExp(Collectable exp, Vector3 spawnPos)
        {
            int rng = Random.Range(0, levelParameters.expPresets.Length);
            exp.expPreset = levelParameters.expPresets[rng];
            exp.UpdateToPreset();
            exp.transform.position = spawnPos;
            
            exp.Initialize(this, gameArea, layerIndex);
        }

        public GameObject[] GetPrefab()
        {
            return prefabs;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class GameSaveManager : MonoBehaviour
    {
        [SerializeField] private UIHandler uiHandler;
        [SerializeField] private Transform infiniteWorldTransform;
        [SerializeField] private PoolManager poolManager;

        private SaveLoadSystem _saveLoadSystem;
        private const string SaveFileName = "save.json";

        private void Awake()
        {
            _saveLoadSystem = new SaveLoadSystem();
            _saveLoadSystem.Initialize();
        }

        private void OnEnable()
        {
            if (uiHandler != null)
            {
                uiHandler.OnSaveGame += SaveGame;
                uiHandler.OnLoadGame += LoadGame;
            }
        }

        private void OnDisable()
        {
            if (uiHandler != null)
            {
                uiHandler.OnSaveGame -= SaveGame;
                uiHandler.OnLoadGame -= LoadGame;
            }
        }

        private void SaveGame()
        {
            Debug.Log("Saving Game...");

            if (infiniteWorldTransform == null || poolManager == null)
            {
                Debug.LogError("Cannot save: World Transform or Pool Manager is missing.");
                return;
            }

            var sessionData = new DataToSave.GameSessionData
            {
                World = new DataToSave.WorldData(infiniteWorldTransform),
                EnemyPoolManager = new DataToSave.EnemyPoolManager(poolManager)
            };

            // Save enemies
            var enemies = poolManager.transform.GetComponentsInChildren(typeof(Enemy), false);
            sessionData.Enemies = new List<DataToSave.EnemyData>();

            foreach (var enemy in enemies)
            {
                // Cast component to GameObject safely
                if (enemy is Component component)
                {
                    sessionData.Enemies.Add(new DataToSave.EnemyData(component.gameObject));
                }
            }

            _saveLoadSystem.Save(sessionData, SaveFileName);
        }

        private void LoadGame()
        {
            Debug.Log("Loading Game...");

            var loadedData = _saveLoadSystem.Load<DataToSave.GameSessionData>(SaveFileName);
            if (loadedData == null)
            {
                Debug.LogWarning("Save data not found or empty.");
                return;
            }

            // 1. Load World Position
            if (infiniteWorldTransform != null)
            {
                Vector3 worldPos = infiniteWorldTransform.position;
                worldPos.x = loadedData.World.WorldPositionX;
                worldPos.y = loadedData.World.WorldPositionY;
                worldPos.z = loadedData.World.WorldPositionZ;
                infiniteWorldTransform.position = worldPos;
            }

            // 2. Reset Enemies
            if (poolManager != null)
            {
                poolManager.DeactivateAllActiveObjects();

                // Restore Pool Manager Position
                if (loadedData.EnemyPoolManager != null)
                {
                    Vector3 poolPos = new Vector3(
                        loadedData.EnemyPoolManager.PoolPositionX,
                        loadedData.EnemyPoolManager.PoolPositionY,
                        loadedData.EnemyPoolManager.PoolPositionZ);
                    poolManager.transform.position = poolPos;
                }

                // 3. Spawn Saved Enemies
                if (loadedData.Enemies != null)
                {
                    foreach (var enemyData in loadedData.Enemies)
                    {
                        Vector3 pos = new Vector3(enemyData.EnemyPositionX, enemyData.EnemyPositionY,
                            enemyData.EnemyPositionZ);
                        poolManager.SpawnFromSave(enemyData.EnemyType, pos);
                    }
                }
            }
            else
            {
                Debug.LogError("Pool Manager reference is missing in GameSaveManager.");
            }
        }
    }
}
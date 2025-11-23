using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class GameSaveManager : MonoBehaviour
    {
        [SerializeField] private UIHandler uiHandler;
        [SerializeField] private Transform infiniteWorldTransform;
        [SerializeField] private PoolManager poolManager;
        [SerializeField] private GameObject poolPrefab;

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
            Debug.Log($"[SAVE DEBUG] PoolManager name: {poolManager.name}");
            Debug.Log($"[SAVE DEBUG] PoolManager child count: {poolManager.transform.childCount}");
            
            var enemies = poolManager.transform.GetComponentsInChildren(typeof(Enemy), true);
            Debug.Log($"[SAVE DEBUG] Found {enemies.Length} Enemy components (including inactive)");
            
            sessionData.Enemies = new List<DataToSave.EnemyData>();

            foreach (var enemy in enemies)
            {
                if (enemy is Component component)
                {
                    Debug.Log($"[SAVE DEBUG] Adding enemy: {component.gameObject.name} at position {component.gameObject.transform.position}");
                    sessionData.Enemies.Add(new DataToSave.EnemyData(component.gameObject));
                }
            }

            Debug.Log($"[SAVE DEBUG] Total enemies saved to JSON: {sessionData.Enemies.Count}");
            _saveLoadSystem.Save(sessionData, SaveFileName);
            Debug.Log("Game saved!");
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
                Debug.Log("Reset Enemies");

                // Deactivate all currently active enemies
                poolManager.DeactivateAllActiveObjects();
                Debug.Log("All active enemies deactivated.");

                // Restore Pool Manager Position
                if (loadedData.EnemyPoolManager != null)
                {
                    Debug.Log("Reset pool manager pos");
                    Vector3 poolPos = new Vector3(
                        loadedData.EnemyPoolManager.PoolPositionX,
                        loadedData.EnemyPoolManager.PoolPositionY,
                        loadedData.EnemyPoolManager.PoolPositionZ);
                    poolManager.transform.position = poolPos;
                }

                // 3. Spawn Saved Enemies
                if (loadedData.Enemies != null)
                {
                    Debug.Log($"Spawning {loadedData.Enemies.Count} saved enemies");
                    foreach (var enemyData in loadedData.Enemies)
                    {
                        Debug.Log("Spawn Saved Enemies");
                        Vector3 pos = new Vector3(enemyData.EnemyPositionX, enemyData.EnemyPositionY,
                            enemyData.EnemyPositionZ);
                        poolManager.SpawnFromSave(enemyData.EnemyType, pos);
                    }
                }
                else
                {
                    Debug.LogError("Enemies loaded data not found or empty.");
                }
            }
            else
            {
                Debug.LogError("Pool Manager reference is missing in GameSaveManager.");
            }
        }
    }
}
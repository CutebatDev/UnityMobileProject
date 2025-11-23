using System;
using System.Collections.Generic;
using System.IO;
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
        private const string SaveFileName = "save";
        private const string SaveFileExtension = ".json";

        public static string SavePath =>
            Application.persistentDataPath + "/" + SaveFileName + saveIndex() + SaveFileExtension;

        private static int saveIndex()
        {
            return System.IO.Directory.GetFiles(Application.persistentDataPath).Length;
        }

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

            var sessionData = new DataToSave.GameSessionData(infiniteWorldTransform, poolManager);
            // Save enemies
            Debug.Log($"[SAVE DEBUG] PoolManager name: {poolManager.name}");
            Debug.Log($"[SAVE DEBUG] PoolManager child count: {poolManager.transform.childCount}");
            Debug.Log($"[SAVE DEBUG] Found {sessionData.Enemies} Enemy components (including inactive)");
            Debug.Log($"[SAVE DEBUG] Total enemies saved to JSON: {sessionData.Enemies.Count}");
            _saveLoadSystem.Save(sessionData, SavePath);
            Debug.Log("Game saved!");
        }

        private void LoadGame()
        {
            Debug.Log("Loading Game...");

            var loadedData = _saveLoadSystem.Load<DataToSave.GameSessionData>(SavePath);
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

        public Dictionary<DataToSave.GameSessionData, Sprite> GetSaveFiles()
        {
            var saveFilesPath = System.IO.Directory.GetFiles(Application.persistentDataPath);
            Dictionary<DataToSave.GameSessionData, Sprite> savesAndScreenshots =
                new Dictionary<DataToSave.GameSessionData, Sprite>();
            foreach (var save in saveFilesPath)
            {
                if (save.Contains(".json"))
                {
                    savesAndScreenshots[(_saveLoadSystem.Load<DataToSave.GameSessionData>(save))] =
                        GetSpriteFromSave(save);
                }
            }

            return savesAndScreenshots;
        }

        private Sprite GetSpriteFromSave(string save)
        {
            save = save.Replace(".json", ".jpeg");
            if (!File.Exists(save))
            {
                return null;
            }

            byte[] imageData = File.ReadAllBytes(save);
            Texture2D texture = new Texture2D(128, 128);
            texture.LoadImage(imageData);
            return Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
        }
    }
}
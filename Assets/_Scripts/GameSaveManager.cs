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
        [SerializeField] private HealthManager playerHealthManager;
        [SerializeField] private int screenshotWidth = 512;
        [SerializeField] private int screenshotHeight = 512;

        private SaveLoadSystem _saveLoadSystem;
        private const string SaveFileName = "save";
        private const string SaveFileExtension = ".json";
        private const string ScreenshotExtension = ".jpeg";

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
            }
        }

        private void OnDisable()
        {
            if (uiHandler != null)
            {
                uiHandler.OnSaveGame -= SaveGame;
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

            var sessionData = new DataToSave.GameSessionData(infiniteWorldTransform, poolManager, playerHealthManager);
            // Save enemies
            Debug.Log($"[SAVE DEBUG] PoolManager name: {poolManager.name}");
            Debug.Log($"[SAVE DEBUG] PoolManager child count: {poolManager.transform.childCount}");
            Debug.Log($"[SAVE DEBUG] Found {sessionData.Enemies} Enemy components (including inactive)");
            Debug.Log($"[SAVE DEBUG] Total enemies saved to JSON: {sessionData.Enemies.Count}");

            int index = saveIndex();
            string savePath = Application.persistentDataPath + "/" + SaveFileName + index + SaveFileExtension;

            _saveLoadSystem.Save(sessionData, savePath);
            TakeAndSaveScreenshot(savePath);
            Debug.Log("Game saved!");
        }

        private void TakeAndSaveScreenshot(string jsonSavePath)
        {
            try
            {
                // Create a render texture
                RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
                Camera.main.targetTexture = rt;

                // Render the camera to the texture
                Camera.main.Render();

                // Read the pixels from the render texture
                RenderTexture.active = rt;
                Texture2D screenshot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
                screenshot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
                screenshot.Apply();

                // Clean up render texture
                Camera.main.targetTexture = null;
                RenderTexture.active = null;
                Destroy(rt);

                // Save as JPEG
                byte[] bytes = ImageConversion.EncodeToJPG(screenshot);
                string screenshotPath = jsonSavePath.Replace(SaveFileExtension, ScreenshotExtension);
                File.WriteAllBytes(screenshotPath, bytes);

                Debug.Log($"Screenshot saved to: {screenshotPath}");

                // Clean up
                Destroy(screenshot);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error taking screenshot: {e.Message}");
            }
        }

        private void LoadGame(DataToSave.GameSessionData gameSessionData)
        {
            Debug.Log("Loading Game...");

            var loadedData = gameSessionData;
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

            // 4. Restore Player stats
            if (loadedData.Player != null)
            {
                playerHealthManager.maxHealth = loadedData.Player.MaxHealth;
                playerHealthManager.currentHealth = loadedData.Player.CurrentHealth;
                ExpManager.Instance.currentLevel = loadedData.Player.CurrentLevel;
                ExpManager.Instance.currentExp = loadedData.Player.CurrentExp;
                ExpManager.Instance.requiredExp = loadedData.Player.RequiredXp;
                ScoreManager.Instance.currentScore = loadedData.Player.CurrentScore;
            }
            else
            {
                Debug.LogError("Player reference is missing in GameSaveManager.");
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

        public void LoadSpecificSave(DataToSave.GameSessionData gameSessionData)
        {
            LoadGame(gameSessionData);
        }
    }
}
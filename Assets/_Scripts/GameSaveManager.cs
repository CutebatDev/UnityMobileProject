using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class GameSaveManager : MonoBehaviour
    {
        [SerializeField] private UIHandler uiHandler;
        private SaveLoadSystem _saveLoadSystem;
        private const string SaveFileName = "save.json";
        public Transform infiniteWorldTransform;
        public PoolManager poolManager;

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
            Debug.Log("Saving...");
            var sessionData = new DataToSave.GameSessionData();
            // Saving the position of the world
            if (infiniteWorldTransform == null)
            {
                Debug.Log("Infinite World is null");
                return;
            }

            sessionData.World = new DataToSave.WorldData(infiniteWorldTransform);

            // Saving enemies data
            if (poolManager == null)
            {
                Debug.Log("Pool Manager is null");
                return;
            }

            var enemies = poolManager.transform.GetComponentsInChildren(typeof(Enemy), false);
            sessionData.Enemies = new List<DataToSave.EnemyData>();
            foreach (var enemy in enemies)
            {
                var data = new DataToSave.EnemyData(enemy.transform);
                sessionData.Enemies.Add(data);
            }

            _saveLoadSystem.Save(sessionData, SaveFileName);
        }

        private void LoadGame()
        {
            Debug.Log("Loading...");

            var loadedData = _saveLoadSystem.Load<DataToSave.GameSessionData>(SaveFileName);
            if (loadedData == null)
            {
                Debug.Log("Data is null");
                return;
            }

            // world loading
            Vector3 vector3 = infiniteWorldTransform.position;
            vector3.x = loadedData.World.WorldPositionX;
            vector3.y = loadedData.World.WorldPositionY;
            vector3.z = loadedData.World.WorldPositionZ;
            infiniteWorldTransform.position = vector3;

            // enemies loading
            var existingEnemies = poolManager.GetComponentsInChildren<Enemy>(false);
            foreach (var enemy in existingEnemies)
            {
                poolManager.ReturnToPool(enemy.gameObject);
            }
        }
    }
}
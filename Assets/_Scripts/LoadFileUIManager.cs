using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class LoadFileUIManager : MonoBehaviour
    {
        [SerializeField] private LoadFileUIHandler loadFileUIHandlerPrefab;
        [SerializeField] private List<LoadFileUIHandler> loadFileUIHandlers;
        [SerializeField] private GameSaveManager gameSaveManager;

        public void Initialize()
        {
            if (gameSaveManager == null)
            {
                Debug.LogError("GameSaveManager is not assigned in the Inspector!");
                return;
            }

            if (loadFileUIHandlers == null)
            {
                loadFileUIHandlers = new List<LoadFileUIHandler>();
            }

            var dictOfSaves = gameSaveManager.GetSaveFiles();

            foreach (var save in dictOfSaves)
            {
                var handler = Instantiate(loadFileUIHandlerPrefab, transform);
                handler.Initialize(save.Key, save.Value);
                loadFileUIHandlers.Add(handler);
            }
        }
    }
}
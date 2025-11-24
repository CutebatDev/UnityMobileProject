using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class LoadFileUIHandler : MonoBehaviour
    {
        [SerializeField] private Image _screenshot;
        private DataToSave.GameSessionData _gameSessionData;
        private GameSaveManager _gameSaveManager;
        private UIHandler _uiHandler;

        public void OnClicked()
        {
            Debug.Log("Button clicked!");

            if (_gameSaveManager == null)
                _gameSaveManager = FindObjectOfType<GameSaveManager>();

            if (_uiHandler == null)
                _uiHandler = FindObjectOfType<UIHandler>();

            if (_gameSessionData != null && _gameSaveManager != null)
            {
                Debug.Log("Loading save and calling ContinueButton...");
                _gameSaveManager.LoadSpecificSave(_gameSessionData);

                if (_uiHandler != null)
                {
                    Debug.Log("Calling ContinueButton");
                    _uiHandler.ContinueButton();
                }
                else
                {
                    Debug.LogError("UIHandler not found!");
                }
            }
            else
            {
                Debug.LogError("Game Session Data or GameSaveManager is missing!");
            }
        }

        public void Initialize(DataToSave.GameSessionData gameSessionData, Sprite sprite)
        {
            _gameSessionData = gameSessionData;
            _screenshot.sprite = sprite;
            _screenshot.gameObject.SetActive(true);
        }
    }
}
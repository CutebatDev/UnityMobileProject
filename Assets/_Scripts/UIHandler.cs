using System;
using UnityEditor;
using UnityEngine;

namespace _Scripts
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject canvasInput;
        [SerializeField] private GameObject canvasPauseMenu;

        private void Start()
        {
            ContinueButton();
        }

        public void ContinueButton()
        {
            canvasInput.SetActive(true);
            canvasPauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void OptionsButton()
        {
            Debug.Log("Options Button");
            // implement options button
        }

        public void QuitButton()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }

        public void PauseButton()
        {
            canvasInput.SetActive(false);
            canvasPauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
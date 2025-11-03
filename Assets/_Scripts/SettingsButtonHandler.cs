using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [Header("Settings Integration")]
    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private Transform uiParent;
    [SerializeField] private Button settingsButton;

    private GameObject currentSettingsPanel;

    private void Start()
    {
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }

        // If no UI parent specified, use Canvas
        if (uiParent == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
                uiParent = canvas.transform;
        }
    }

    public void OpenSettings()
    {
        if (currentSettingsPanel == null && settingsPanelPrefab != null)
        {
            currentSettingsPanel = Instantiate(settingsPanelPrefab, uiParent);
            
            // Get the SettingsUI component and call OpenSettings
            SettingsUI settingsUI = currentSettingsPanel.GetComponent<SettingsUI>();
            if (settingsUI != null)
            {
                settingsUI.OpenSettings();
            }
        }
        else if (currentSettingsPanel != null)
        {
            currentSettingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (currentSettingsPanel != null)
        {
            currentSettingsPanel.SetActive(false);
        }
    }
}

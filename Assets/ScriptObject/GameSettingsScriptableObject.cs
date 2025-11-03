using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float soundVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Header("Joystick Settings")]
    [Range(0.5f, 3f)]
    public float joystickSensitivity = 1f; // For Endless Survival

    [Header("Display Settings")]
    public bool orientationAdaptationEnabled = true;

    public enum ControlScheme
    {
        Swipe,
        Buttons
    }
}

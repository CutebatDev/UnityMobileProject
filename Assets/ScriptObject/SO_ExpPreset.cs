using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "ExpPreset", menuName = "Scriptable Objects/ExpPresets")]
    public class SO_ExpPreset : ScriptableObject
    {
        public int value;
        public float scaleModifier;
    }
}
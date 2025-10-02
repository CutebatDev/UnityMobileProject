using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "SO_PlayerPresets", menuName = "Scriptable Objects/PlayerPresets")]
    //Player starting Stats SO: Speed, health, damage, read by Player Actions
    public class SO_PlayerPresets : ScriptableObject
    {
        public float speed;
        public int health;
        public int damage;
    }
}

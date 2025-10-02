using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "SO_EnemyPreset", menuName = "Scriptable Objects/EnemyPreset")]
    public class SO_EnemyPreset : ScriptableObject
    {
        public float speed;
        public float health;
        public float damage;
        public float sizeModifier;
    }
}

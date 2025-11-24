using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "BulletPreset", menuName = "Scriptable Objects/BulletPreset")]
    public class SO_BulletPreset : ScriptableObject
    {
        public int bulletDamage;
        public float bulletSpeed;
    }
}
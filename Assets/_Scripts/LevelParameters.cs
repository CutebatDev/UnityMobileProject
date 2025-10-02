using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "LevelParameters", menuName = "Scriptable Objects/LevelParameters")]
    public class LevelParameters : ScriptableObject
    {
        [Header("Player")] [SerializeField] public int playerSpeed;
        [SerializeField] public int playerHealth;
        [SerializeField] public int playerDamage;
    }
}
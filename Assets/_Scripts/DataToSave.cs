using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    public class DataToSave
    {
        [Serializable]
        public class GameSessionData
        {
            public WorldData World;
            public List<EnemyData> Enemies;
            public EnemyPoolManager EnemyPoolManager;
            public PlayerData Player;

            public GameSessionData()
            {
            }

            public GameSessionData(Transform world, PoolManager enemyPoolManager, HealthManager playerHealth)
            {
                World = new WorldData(world);
                EnemyPoolManager = new EnemyPoolManager(enemyPoolManager);
                Enemies = Enemy.enemies.Select(enemy => new EnemyData(enemy)).ToList();
                Player = new PlayerData(playerHealth.maxHealth, playerHealth.currentHealth);
            }

            /* Enemies = Enemy.enemies.Select(enemy => new EnemyData(enemy)).ToList(); ->
             -> public EnemyData CreateEnemyData(Enemy enemy)
            {
                return new EnemyData(enemy);
            }*/
        }

        [Serializable]
        public class WorldData
        {
            public float WorldPositionX { get; set; }
            public float WorldPositionY { get; set; }
            public float WorldPositionZ { get; set; }

            public WorldData()
            {
            }

            public WorldData(Transform worldTransform)
            {
                WorldPositionX = worldTransform.position.x;
                WorldPositionY = worldTransform.position.y;
                WorldPositionZ = worldTransform.position.z;
            }
        }

        [Serializable]
        public class PlayerData
        {
            public float MaxHealth { get; set; }
            public float CurrentHealth { get; set; }
            public int CurrentLevel { get; set; }
            public int CurrentExp { get; set; }
            public int RequiredXp { get; set; }
            public int CurrentScore { get; set; }

            public PlayerData(float maxHealth, float currentHealth)
            {
                MaxHealth = maxHealth;
                CurrentHealth = currentHealth;
                CurrentLevel = ExpManager.Instance.currentLevel;
                CurrentExp = ExpManager.Instance.currentExp;
                RequiredXp = ExpManager.Instance.requiredExp;
                CurrentScore = ScoreManager.Instance.currentScore;
            }
        }

        [Serializable]
        public class EnemyData
        {
            public string EnemyType { get; set; }
            public float EnemyPositionX { get; set; }
            public float EnemyPositionY { get; set; }
            public float EnemyPositionZ { get; set; }
            public float MaxHealth { get; set; }
            public float CurrentHealth { get; set; }

            public EnemyData()
            {
            }

            public EnemyData(Enemy enemy)
            {
                // Clean up the name to remove (Clone), (1), etc.
                string cleanName = enemy.name.Replace("(Clone)", "");
                int parenIndex = cleanName.IndexOf('(');
                if (parenIndex > 0)
                {
                    cleanName = cleanName.Substring(0, parenIndex);
                }

                EnemyType = cleanName.Trim();

                EnemyPositionX = enemy.transform.position.x;
                EnemyPositionY = enemy.transform.position.y;
                EnemyPositionZ = enemy.transform.position.z;
                Debug.Log($"[SAVE DEBUG] HealthManager name: {enemy.healthManager == null}");
                MaxHealth = enemy.healthManager.maxHealth;
                CurrentHealth = enemy.healthManager.currentHealth;
            }
        }

        public class EnemyPoolManager
        {
            public float PoolPositionX { get; set; }
            public float PoolPositionY { get; set; }
            public float PoolPositionZ { get; set; }

            public EnemyPoolManager()
            {
            }

            public EnemyPoolManager(PoolManager enemyPoolTransform)
            {
                PoolPositionX = enemyPoolTransform.transform.position.x;
                PoolPositionY = enemyPoolTransform.transform.position.y;
                PoolPositionZ = enemyPoolTransform.transform.position.z;
            }
        }
    }
}
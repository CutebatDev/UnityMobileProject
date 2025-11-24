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

            public GameSessionData()
            {
            }

            public GameSessionData(Transform world, PoolManager enemyPoolManager)
            {
                World = new WorldData(world);
                EnemyPoolManager = new EnemyPoolManager(enemyPoolManager);
                Enemies = Enemy.enemies.Select(enemy => new EnemyData(enemy)).ToList();
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
            public int Health { get; set; }
            public int Lives { get; set; }
            public int Score { get; set; }

            public PlayerData(int health, int lives, int score)
            {
                Health = health;
                Lives = lives;
                Score = score;
            }
        }

        [Serializable]
        public class EnemyData
        {
            public string EnemyType { get; set; }
            public float EnemyPositionX { get; set; }
            public float EnemyPositionY { get; set; }
            public float EnemyPositionZ { get; set; }

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
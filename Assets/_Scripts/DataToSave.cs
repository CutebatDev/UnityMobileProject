using System;
using System.Collections.Generic;
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

            public GameSessionData()
            {
                Enemies = new List<EnemyData>();
            }
        }

        [Serializable]
        public class WorldData
        {
            public float WorldPositionX { get; set; }
            public float WorldPositionY { get; set; }
            public float WorldPositionZ { get; set; }

            // Json needs this to instantiate the object without a Transform
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
            public float EnemyPositionX { get; set; }
            public float EnemyPositionY { get; set; }
            public float EnemyPositionZ { get; set; }

            // Json needs this to instantiate the object without a Transform
            public EnemyData()
            {
            }

            public EnemyData(Transform enemyPosition)
            {
                EnemyPositionX = enemyPosition.position.x;
                EnemyPositionY = enemyPosition.position.y;
                EnemyPositionZ = enemyPosition.position.z;
            }
        }
    }
}
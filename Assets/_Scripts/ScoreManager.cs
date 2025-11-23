using UnityEngine;

namespace _Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        // For survival game : 
        // Score per second alive
        // Score per enemy defeated

        public static ScoreManager Instance { get; private set; }
        
        public int currentScore = 0;
        public int scorePerSecond = 1;
        private float _nextScoreTickTime = 0.0f;
        public float scoreTickLength = 1.0f; // in seconds **change this if 
        // Scriptable Object for difficulty;
    
        void Start()
        {
            if (Instance == null && Instance != this)
                Instance = this;
            /*
            * Set score per second from SC
            */
        }

        void Update()
        {
            scoreTickLength = 1.0f * Time.timeScale; 
            if (Time.time >= _nextScoreTickTime ) {
                _nextScoreTickTime += scoreTickLength;
                currentScore += scorePerSecond;
            }
        }
    
        public void AddScore(int amount) // for kills
        {
            currentScore += amount;
        }
    }
}

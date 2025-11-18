using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // For survival game : 
    // Score per second alive
    // Score per enemy defeated

    public int currentScore = 0;
    public int scorePerSecond = 1;
    private float _nextScoreTickTime = 0.0f;
    public float scoreTickLength = 1.0f; // in seconds **change this if 
    // public ScoreScriptableObject name;
    
    void Start()
    {
        /*
         * Set values from SO
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

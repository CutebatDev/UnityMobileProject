using System;
using System.Collections.Generic;
using UnityEngine;
/*
    Endless Survival: Track time survived + kills
    Display score on HUD 
 */
namespace _Scripts
{
    public enum ScoreFields
    {
        TimeSurvived,
        EnemiesKilled
    }
    public class ScoringSystem : MonoBehaviour
    {
        public float timeSurvived = 0;
        public int killed = 0;

        private bool _trackTime = false;

        private void OnEnable()
        {
            _trackTime = true;
        }

        private void OnDisable()
        {
            _trackTime = false;
        }

        private void Update()
        {
            if(_trackTime)
                timeSurvived += Time.deltaTime;
        }

        public void AddKill()
        {
            killed++;
        }

        public Dictionary<ScoreFields, String> GetScores()
        {
            var temp =  new Dictionary<ScoreFields, String>();
            
            int min = (int)timeSurvived / 60;
            int sec = (int)timeSurvived % 60;
            temp[ScoreFields.TimeSurvived] = $"{min}:{sec}";
            
            temp[ScoreFields.EnemiesKilled] = $"{killed}";
            return temp;
        }
    }
}

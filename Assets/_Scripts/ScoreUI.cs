using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace _Scripts
{
    public class ScoreUI : MonoBehaviour
    {
        private ScoreManager _scoreManager;
        public TMP_Text textMesh;
        private void Start()
        {
            _scoreManager = ScoreManager.Instance;
        }

        private void Update()
        {
            textMesh.text = $"SCORE: {ScoreManager.Instance.currentScore}";
            
        }
    }
}
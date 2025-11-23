using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace _Scripts
{
    public class ScoreUI : MonoBehaviour
    {
        ScoreManager scoreManager;
        private TextMeshPro textMesh;
        private void Start()
        {
            scoreManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
        }

        private void Update()
        {
            textMesh.text = $"Score : {scoreManager.currentScore}";
        }
    }
}
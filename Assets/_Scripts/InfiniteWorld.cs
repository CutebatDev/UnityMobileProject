using System;
using System.Collections;
using NUnit.Framework.Internal.Execution;
using UnityEngine;

namespace _Scripts
{
    public class InfiniteWorld : MonoBehaviour
    {
        [SerializeField] private LevelParameters levelParams;
        [SerializeField] private Transform playerTransform;
        private void OnEnable() => InputHandler.OnMove += Movement;
        private void OnDisable() => InputHandler.OnMove -= Movement;

        private void Start()
        {
            StartCoroutine(CheckBounds());
        }

        private void Movement(float horizontal = 0f, float vertical = 0f)
        {
            Vector3 dir = new Vector3(-horizontal, 0f, -vertical); // top down reversed movement
            transform.position += dir * (levelParams.playerSpeed * Time.deltaTime);
        }

        private IEnumerator CheckBounds()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (transform.position.x > levelParams.worldBounds[0] ||
                    transform.position.z > levelParams.worldBounds[2] ||
                    transform.position.x < -levelParams.worldBounds[0] ||
                    transform.position.z < -levelParams.worldBounds[2])
                {
                    SaveAndRepositionWorldObjects();
                }
            }
        }

        private void SaveAndRepositionWorldObjects()
        {
            foreach (Transform child in transform)
            {
                if (!child.CompareTag("Ground"))
                {
                    Vector3 worldPos = child.position;
                    transform.position = Vector3.zero;
                    child.position = new Vector3(playerTransform.position.x + worldPos.x, worldPos.y,
                        playerTransform.position.z + worldPos.z);
                }
            }
        }
    }
}
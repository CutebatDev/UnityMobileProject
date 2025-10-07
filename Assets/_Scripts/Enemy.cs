using _Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LevelParameters levelParams;

    private void Update()
    {
        MovementToPlayer();
    }

    private void MovementToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0; // keep y axis unchanged
        transform.Translate(dir * (levelParams.enemySpeed * Time.deltaTime));
    }
}
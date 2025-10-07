using _Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LevelParameters levelParams;
    private GameArea _gameArea;
    private int _layerIndex;

    public void Initialize(SpawnManager owner, GameArea gameArea, int layerIndex)
    {
        _gameArea = gameArea;
        _layerIndex = layerIndex;
    }

    void FixedUpdate()
    {
        MovementToPlayer();

        if (_gameArea != null &&
            _gameArea.IsOutOfBounds(transform.position, _gameArea.layers[_layerIndex]))
        {
            gameObject.SetActive(false);
        }
    }

    private void MovementToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0; // keep y axis unchanged
        transform.Translate(dir * (levelParams.enemySpeed * Time.deltaTime));
    }
}
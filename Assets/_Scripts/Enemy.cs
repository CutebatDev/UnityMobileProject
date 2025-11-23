using System;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LevelParameters levelParams;
    private GameArea _gameArea;
    private int _layerIndex;
    public static List<Enemy> enemies = new List<Enemy>();


    // SO PARAMS

    public SO_EnemyPreset Preset;

    #region Enemy Parameters

    [HideInInspector] public float speed = 1;

    [HideInInspector] public float health = 10;

    [HideInInspector] public float damage = 2;

    #endregion

    private void OnEnable()
    {
        enemies.Add(this);
    }

    private void OnDisable()
    {
        enemies.Remove(this);
    }

    private void Awake()
    {
        speed = Preset.speed;
        health = Preset.health;
        damage = Preset.damage;
        transform.localScale *= Preset.sizeModifier;
    }

    // SO PARAMS END

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
        if (player == null)
            return;
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0; // keep y axis unchanged
        transform.Translate(dir * (levelParams.enemySpeed * Time.deltaTime));
    }
}
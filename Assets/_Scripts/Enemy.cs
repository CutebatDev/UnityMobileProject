using System;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LevelParameters levelParams;
    private GameArea _gameArea;
    private int _layerIndex;
    [SerializeField] private GameObject mesh;
    
	
	public SO_EnemyPreset enemyPreset;
    public LevelParameters difficulty;
    
    [HideInInspector] public float speed = 1;
    [HideInInspector] public float damage = 2;
    [HideInInspector] public int scoreReward = 10;

    private HealthManager _healthManager;


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
        _healthManager = GetComponent<HealthManager>();
    }

    public void UpdateToPreset()
    {
        if(enemyPreset)
        {
            _healthManager.currentHealth =  enemyPreset.health * difficulty.enemyHealthModifier;
            _healthManager.maxHealth =      enemyPreset.health * difficulty.enemyHealthModifier;
            speed =                         enemyPreset.speed * difficulty.enemySpeedModifier;
            damage =                        enemyPreset.damage * difficulty.enemyDamageModifier;
            scoreReward =                   (int)(enemyPreset.scoreReward * difficulty.enemyScoreRewardModifier);
            transform.localScale =          Vector3.one * enemyPreset.sizeModifier;
        }
    }

    public void Initialize(SpawnManager owner, GameArea gameArea, int layerIndex)
    {
        _gameArea = gameArea;
        _layerIndex = layerIndex;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        MovementToPlayer();

        if (_gameArea != null &&
            _gameArea.IsOutOfBounds(transform.position, _gameArea.layers[_layerIndex]))
        {
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }

    private void MovementToPlayer()
    {
        if (player == null)
            return;
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0; // keep y axis unchanged
        transform.Translate(dir * (levelParams.enemySpeedModifier * Time.deltaTime));
        mesh.transform.rotation = Quaternion.LookRotation(dir);
    }
}

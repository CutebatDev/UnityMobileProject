using System;
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


	// SO PARAMS
	
	public SO_EnemyPreset Preset;

    #region Enemy Parameters

    [HideInInspector]
    public float speed = 1;
    
    [HideInInspector]
    public float damage = 2;

    private HealthManager healthManager;
    
    #endregion


    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        if (Preset == null)
            Preset = Resources.Load<SO_EnemyPreset>("SO_EnemyPreset_NormalEnemy");
        speed = Preset.speed;
        healthManager.currentHealth = Preset.health;
        damage = Preset.damage;
        transform.localScale = Vector3.one * Preset.sizeModifier;
    }

    public void UpdateToPreset()
    {
        if(Preset != null)
        {
            speed = Preset.speed;
            healthManager.currentHealth = Preset.health;
            damage = Preset.damage;
            transform.localScale = Vector3.one * Preset.sizeModifier;
        }
    }
	// SO PARAMS END

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
            gameObject.SetActive(false);
        }
    }

    private void MovementToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0; // keep y axis unchanged
        transform.Translate(dir * (levelParams.enemySpeed * Time.deltaTime));
        mesh.transform.rotation = Quaternion.LookRotation(dir);
    }
    
}

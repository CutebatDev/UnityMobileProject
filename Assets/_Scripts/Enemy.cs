using System;
using _Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SO_EnemyPreset Preset;

    #region Enemy Parameters

    [HideInInspector]
    public float speed = 1;
    
    [HideInInspector]
    public float health = 10;
    
    [HideInInspector]
    public float damage = 2;

    #endregion


    private void Awake()
    {
        speed = Preset.speed;
        health = Preset.health;
        damage = Preset.damage;
        transform.localScale *=  Preset.sizeModifier;
    }
}

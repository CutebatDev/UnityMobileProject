using System;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnLayer
{
    public string name = "Default";
    public float distanceToCamera = 10f;

    public float padding = 2f;

    // cached values
    [HideInInspector] public Vector3 minBounds;
    [HideInInspector] public Vector3 maxBounds;
}

public class GameArea : MonoBehaviour
{
    public Camera camera;
    public SpawnLayer[] layers;
    [SerializeField] private Transform terrainPos;

    void Start()
    {
        RecalculateAllLayers();
    }

    void OnValidate()
    {
        if (layers != null && layers.Length > 0)
            RecalculateAllLayers();
    }

    public void RecalculateAllLayers()
    {
        foreach (var layer in layers)
        {
            RecalculateLayerBounds(layer);
        }
    }

    void RecalculateLayerBounds(SpawnLayer layer)
    {
        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, layer.distanceToCamera));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, layer.distanceToCamera));

        layer.minBounds = new Vector3(bottomLeft.x, bottomLeft.z, bottomLeft.y);
        layer.maxBounds = new Vector3(topRight.x, topRight.z, topRight.y);
    }

    public Vector3 GetSpawnPosition(SpawnLayer layer)
    {
        float x = 0;
        float y = terrainPos.position.y;
        float z = 0;
        switch (Random.Range(2, 4))
        {
            case 0: // top
                x = Random.Range(layer.minBounds.x, layer.maxBounds.x);
                z = layer.maxBounds.y + layer.padding;
                break;
            case 1: // bottom
                x = Random.Range(layer.minBounds.x, layer.maxBounds.x);
                z = layer.minBounds.y - layer.padding;
                break;
            case 2: // right
                x = layer.maxBounds.y + layer.padding;
                z = Random.Range(layer.minBounds.x, layer.maxBounds.x);
                break;
            case 3: // left
                x = layer.minBounds.y - layer.padding;
                z = Random.Range(layer.minBounds.x, layer.maxBounds.x);
                break;
        }

        return new Vector3(x, y, z);
    }

    public bool IsOutOfBounds(Vector3 pos, SpawnLayer layer)
    {
        return pos.x < layer.minBounds.x - layer.padding ||
               pos.x > layer.maxBounds.x + layer.padding ||
               pos.z < layer.minBounds.y - layer.padding ||
               pos.z > layer.maxBounds.y + layer.padding;
    }

    private void OnDrawGizmos()
    {
        if (camera == null) camera = Camera.main;
        if (layers == null) return;

        if (!Application.isPlaying && layers.Length > 0)
            RecalculateAllLayers();

        foreach (var layer in layers)
        {
            Gizmos.color = Color.yellow;

            // rectangle in XZ, elevated at the layer's Y
            Vector3 bl = new Vector3(layer.minBounds.x, layer.minBounds.z, layer.minBounds.y);
            Vector3 br = new Vector3(layer.maxBounds.x, layer.minBounds.z, layer.minBounds.y);
            Vector3 tr = new Vector3(layer.maxBounds.x, layer.minBounds.z, layer.maxBounds.y);
            Vector3 tl = new Vector3(layer.minBounds.x, layer.minBounds.z, layer.maxBounds.y);

            Gizmos.DrawLine(bl, br);
            Gizmos.DrawLine(br, tr);
            Gizmos.DrawLine(tr, tl);
            Gizmos.DrawLine(tl, bl);
        }
    }
}
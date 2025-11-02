using System;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnLayer
{
    public string name = "Default";
    public float distanceToCamera = 10f;

    public float padding = 2f;
    
    // Store original padding to avoid compounding multipliers
    [HideInInspector] public float originalPadding;

    // cached values
    [HideInInspector] public Vector3 minBounds;
    [HideInInspector] public Vector3 maxBounds;
}

public class GameArea : MonoBehaviour
{
    public Camera camera;
    public SpawnLayer[] layers;
    [SerializeField] private Transform terrainPos;

    [SerializeField] private float leftBorder = 0;
    [SerializeField] private float bottomBorder = 0;
    [SerializeField] private float rightBorder = 1;
    [SerializeField] private float topBorder = 1;

    [Header("Orientation Adaptation")]
    [SerializeField] private bool adaptToOrientation = true;
    [SerializeField] private float portraitPaddingMultiplier = 1.0f;
    [SerializeField] private float landscapePaddingMultiplier = 1.5f;

    void Start()
    {
        // Store original padding values
        foreach (var layer in layers)
        {
            if (layer.originalPadding == 0) // Only set if not already set
                layer.originalPadding = layer.padding;
        }
        
        RecalculateAllLayers();
    }

    void OnValidate()
    {
        // Store original padding values in editor
        if (layers != null)
        {
            foreach (var layer in layers)
            {
                if (layer.originalPadding == 0) // Only set if not already set
                    layer.originalPadding = layer.padding;
            }
            
            if (layers.Length > 0)
                RecalculateAllLayers();
        }
    }

    public void RecalculateAllLayers()
    {
        foreach (var layer in layers)
        {
            RecalculateLayerBounds(layer);
        }
        
        Debug.Log($"GameArea bounds recalculated for {(IsLandscapeOrientation() ? "Landscape" : "Portrait")} - Width: {GetAreaWidth():F2}, Height: {GetAreaHeight():F2}");
    }

    void RecalculateLayerBounds(SpawnLayer layer)
    {
        Vector3 bottomLeft =
            camera.ViewportToWorldPoint(new Vector3(leftBorder, bottomBorder, camera.transform.position.y));
        Vector3 topRight =
            camera.ViewportToWorldPoint(new Vector3(rightBorder, topBorder, camera.transform.position.y));

        layer.minBounds = new Vector3(bottomLeft.x, bottomLeft.z, bottomLeft.y);
        layer.maxBounds = new Vector3(topRight.x, topRight.z, topRight.y);
        
        // Reset padding to original value, then apply orientation multiplier
        if (adaptToOrientation)
        {
            float paddingMultiplier = IsLandscapeOrientation() ? landscapePaddingMultiplier : portraitPaddingMultiplier;
            layer.padding = layer.originalPadding * paddingMultiplier;
        }
        else
        {
            layer.padding = layer.originalPadding;
        }
    }

    public Vector3 GetSpawnPosition(SpawnLayer layer)
    {
        float x = 0;
        float y = terrainPos.position.y + 0.5f;
        float z = 0;
        
        // Get current spawn area dimensions
        float areaWidth = layer.maxBounds.x - layer.minBounds.x;
        float areaHeight = layer.maxBounds.y - layer.minBounds.y;
        
        // Adjust spawn logic based on orientation
        int spawnSide = GetWeightedSpawnSide(areaWidth, areaHeight);
        
        switch (spawnSide)
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
                x = layer.maxBounds.x + layer.padding;
                z = Random.Range(layer.minBounds.y, layer.maxBounds.y);
                break;
            case 3: // left
                x = layer.minBounds.x - layer.padding;
                z = Random.Range(layer.minBounds.y, layer.maxBounds.y);
                break;
        }

        return new Vector3(x, y, z);
    }
    
    private int GetWeightedSpawnSide(float areaWidth, float areaHeight)
    {
        if (!adaptToOrientation)
            return Random.Range(0, 4); // Equal probability for all sides
            
        bool isLandscape = IsLandscapeOrientation();
        
        if (isLandscape)
        {
            // In landscape, favor left/right spawning since we have more width
            float rand = Random.Range(0f, 1f);
            if (rand < 0.6f) // 60% chance for sides
                return Random.Range(2, 4); // left or right
            else // 40% chance for top/bottom
                return Random.Range(0, 2); // top or bottom
        }
        else
        {
            // In portrait, favor top/bottom spawning since we have more height
            float rand = Random.Range(0f, 1f);
            if (rand < 0.6f) // 60% chance for top/bottom
                return Random.Range(0, 2); // top or bottom
            else // 40% chance for sides
                return Random.Range(2, 4); // left or right
        }
    }

    public bool IsOutOfBounds(Vector3 pos, SpawnLayer layer)
    {
        return pos.x < layer.minBounds.x - layer.padding ||
               pos.x > layer.maxBounds.x + layer.padding ||
               pos.z < layer.minBounds.y - layer.padding ||
               pos.z > layer.maxBounds.y + layer.padding;
    }
    
    private bool IsLandscapeOrientation()
    {
        return Screen.orientation == ScreenOrientation.LandscapeLeft || 
               Screen.orientation == ScreenOrientation.LandscapeRight;
    }
    
    private float GetAreaWidth()
    {
        if (layers.Length > 0)
            return layers[0].maxBounds.x - layers[0].minBounds.x;
        return 0f;
    }
    
    private float GetAreaHeight()
    {
        if (layers.Length > 0)
            return layers[0].maxBounds.y - layers[0].minBounds.y;
        return 0f;
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
            
            // Draw spawn padding area
            Gizmos.color = Color.red;
            Vector3 blPad = new Vector3(layer.minBounds.x - layer.padding, layer.minBounds.z, layer.minBounds.y - layer.padding);
            Vector3 brPad = new Vector3(layer.maxBounds.x + layer.padding, layer.minBounds.z, layer.minBounds.y - layer.padding);
            Vector3 trPad = new Vector3(layer.maxBounds.x + layer.padding, layer.minBounds.z, layer.maxBounds.y + layer.padding);
            Vector3 tlPad = new Vector3(layer.minBounds.x - layer.padding, layer.minBounds.z, layer.maxBounds.y + layer.padding);

            Gizmos.DrawLine(blPad, brPad);
            Gizmos.DrawLine(brPad, trPad);
            Gizmos.DrawLine(trPad, tlPad);
            Gizmos.DrawLine(tlPad, blPad);
        }
    }
}
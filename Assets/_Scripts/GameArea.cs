using UnityEngine;

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

    void Start()
    {
        if (camera == null)
            camera = Camera.main;

        RecalculateAllLayers();
    }

    void OnValidate()
    {
        if (camera == null) camera = Camera.main;
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

    public Vector3 GetSpawnPositionFromTop(SpawnLayer layer)
    {
        float x = Random.Range(layer.minBounds.x, layer.maxBounds.x);
        float y = camera.transform.position.y - layer.distanceToCamera;
        float z = layer.maxBounds.y + layer.padding;
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
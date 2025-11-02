using UnityEngine;

namespace _Scripts
{
    public class OrientationManager : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private GameArea gameArea;
        
        [Header("Camera Settings")]
        [SerializeField] private float portraitSize = 5f;
        [SerializeField] private float landscapeSize = 7f;
        
        private bool wasLandscape;
        private float lastScreenWidth;
        private float lastScreenHeight;
        
        void Start()
        {
            // Only run in builds, not in editor
            if (!Application.isPlaying || Application.isEditor)
                return;
                
            if (targetCamera == null)
                targetCamera = Camera.main;
                
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            wasLandscape = IsCurrentlyLandscape();
            
            Debug.Log($"OrientationManager initialized - {Screen.width}x{Screen.height}, Landscape: {wasLandscape}");
            
            AdjustCameraForOrientation();
        }
        
        void Update()
        {
            // Check if screen dimensions changed
            bool dimensionsChanged = Screen.width != lastScreenWidth || Screen.height != lastScreenHeight;
            bool currentlyLandscape = IsCurrentlyLandscape();
            
            if (dimensionsChanged || currentlyLandscape != wasLandscape)
            {
                Debug.Log($"Orientation change: {lastScreenWidth}x{lastScreenHeight} -> {Screen.width}x{Screen.height}");
                Debug.Log($"Landscape: {wasLandscape} -> {currentlyLandscape}");
                
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
                wasLandscape = currentlyLandscape;
                
                AdjustCameraForOrientation();
            }
        }
        
        private void AdjustCameraForOrientation()
        {
            bool isLandscape = IsCurrentlyLandscape();
            
            if (targetCamera.orthographic)
            {
                targetCamera.orthographicSize = isLandscape ? landscapeSize : portraitSize;
            }
            else
            {
                targetCamera.fieldOfView = isLandscape ? 60f : 45f;
            }
            
            Debug.Log($"Camera adjusted for {(isLandscape ? "Landscape" : "Portrait")} - Size: {targetCamera.orthographicSize}");
            
            // Trigger GameArea to recalculate bounds
            if (gameArea != null)
            {
                Invoke(nameof(RecalculateGameArea), 0.1f);
            }
        }
        
        private void RecalculateGameArea()
        {
            gameArea.RecalculateAllLayers();
        }
        
        private bool IsCurrentlyLandscape()
        {
            return Screen.width > Screen.height;
        }
        
        // Public method to manually trigger orientation update (for testing)
        [ContextMenu("Force Orientation Update")]
        public void ForceOrientationUpdate()
        {
            AdjustCameraForOrientation();
        }
    }
}
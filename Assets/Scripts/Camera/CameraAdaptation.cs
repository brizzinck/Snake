using UnityEngine;

public class CameraAdaptation : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector2 baseResolution = new Vector2(1920, 1080);
    private float baseSize = 20f;

    private void Start()
    {
        SetCameraSize();
    }

    private void SetCameraSize()
    {
        float baseAspect = baseResolution.x / baseResolution.y;
        float screenAspect = (float)Screen.width / Screen.height;
        float newSize = baseSize * (baseAspect / screenAspect) / 3f;
        mainCamera.orthographicSize = newSize;
    }
}

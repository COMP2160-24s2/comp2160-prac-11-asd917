using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private Camera mainCamera;
    private Actions action;
    private InputAction zoomAction;

    [SerializeField] private float zoomSpeed = 0.1f;

    [SerializeField] private float minOrthographicSize = 5f;

    [SerializeField] private float maxOrthographicSize = 20f;

    [SerializeField] private float minFieldOfView = 20f;

    [SerializeField] private float maxFieldOfView = 60f;

    void Awake()
    {
        mainCamera = Camera.main;

        action = new Actions();

        zoomAction = action.camera.zoom;
    }

    void OnEnable()
    {
        zoomAction.Enable();

        zoomAction.performed += OnZoom;
    }

    void OnDisable()
    {
        zoomAction.Disable();
        
        zoomAction.performed -= OnZoom;
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();

        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = Mathf.Clamp(
                mainCamera.orthographicSize - scrollValue * zoomSpeed,
                minOrthographicSize, maxOrthographicSize);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Clamp(
                mainCamera.fieldOfView - scrollValue * zoomSpeed,
                minFieldOfView, maxFieldOfView);
        }
    }
}

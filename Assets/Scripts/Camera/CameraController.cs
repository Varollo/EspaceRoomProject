using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] string _firstCamera;
    [SerializeField] private bool _autoGetCameras;
    [SerializeField] private CinemachineVirtualCamera[] _cameras;

    public Dictionary<string, CinemachineVirtualCamera> Cameras { get; private set; }

    public string ActiveCamera
    {
        get => _activeCamera;
        set
        {
            _activeCamera = value;
            UpdateCameraPriority();
        }
    }

    private string _activeCamera;

    private void Start()
    {
        Cameras = new Dictionary<string, CinemachineVirtualCamera>();

        if (_autoGetCameras)
        {
            _cameras = FindObjectsOfType<CinemachineVirtualCamera>();
        }

        foreach (CinemachineVirtualCamera cam in _cameras)
        {
            Cameras.Add(cam.name, cam);
        }

        ActiveCamera = _firstCamera;
    }

    private void UpdateCameraPriority()
    {
        foreach (CinemachineVirtualCamera cam in Cameras.Values)
        {
            cam.Priority = 0;
        }

        if (Cameras.TryGetValue(ActiveCamera, out CinemachineVirtualCamera currentCam))
        {
            currentCam.Priority = 1;
        }
        else
        {
            Debug.LogWarning($"No camera with the name of {ActiveCamera}.");
        }
    }
}

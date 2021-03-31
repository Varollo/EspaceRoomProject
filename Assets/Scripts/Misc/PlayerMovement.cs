using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private int _nCameras;

    public static bool CanMove { get; set; }

    public int CameraIndex { get => (_cameraIndex % _nCameras) + (_cameraIndex < 0 ? _nCameras : 0); private set => _cameraIndex = value; }

    private int _cameraIndex;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveAmount(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveAmount(-1);
        }
    }

    public void MoveAmount(int amt)
    {
        if (!CanMove) return;

        CameraIndex += amt;

        UpdateActiveCamera();
    }

    private void UpdateActiveCamera()
    {
        _cameraController.ActiveCamera = $"Camera{CameraIndex.ToString("00")}";
    }

    public void MoveToCamera(int cam)
    {
        if (!CanMove) return;

        CameraIndex = cam;
        UpdateActiveCamera();
    }
}

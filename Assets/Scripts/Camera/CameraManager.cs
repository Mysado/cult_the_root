using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;


public enum CameraLocation
{
    Down,
    Middle,
    Up,
}
[System.Serializable]
public class CameraPositionParameters
{
    public Transform cameraPosition;
    public float cameraSize;
    public float transitionDuration;
}
public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<CameraPositionParameters> cameraPositions;
    [SerializeField] private Camera mainCamera;

    public event Action OnCameraReachedSurfaceAfterSacrifice; 

    private CameraLocation cameraLocation;
    private bool isInTransition;

    void Start()
    {
        cameraLocation = CameraLocation.Up;
        MoveCamera();
    }

    void Update()
    {
        if(isInTransition)
            return;
        if (Input.mouseScrollDelta.y > 0 && cameraLocation != CameraLocation.Up)
        {
            isInTransition = true;
            cameraLocation++;
            MoveCamera();
            return;

        }
        else if (Input.mouseScrollDelta.y < 0 && cameraLocation != CameraLocation.Down)
        {
            isInTransition = true;
            cameraLocation--;
            MoveCamera();
            return;
        }
    }

    private void MoveCamera(bool overrideTransition = false, float overrideDuration = 0)
    {
        isInTransition = true;
        var cameraPositionParameter = cameraPositions[(int)cameraLocation];
        var transitionDuration = cameraPositionParameter.transitionDuration;
        if (overrideTransition)
            transitionDuration = overrideDuration;
        mainCamera.transform.DOMove(cameraPositions[(int)cameraLocation].cameraPosition.position, transitionDuration).onComplete = () => isInTransition = false;
        mainCamera.DOOrthoSize(cameraPositionParameter.cameraSize, transitionDuration);
    }

    private void OnComplete()
    {
        isInTransition = false;
        OnCameraReachedSurfaceAfterSacrifice?.Invoke();
    }

    public void ChangeCameraLocation(CameraLocation newLocation, bool overrideTransition = false, float overrideDuration = 0)
    {
        cameraLocation = newLocation;
        MoveCamera(overrideTransition, overrideDuration);
    }

    public void MoveCameraToSurface()
    {
        cameraLocation = CameraLocation.Up;
        var cameraPositionParameter = cameraPositions[(int)cameraLocation];
        var transitionDuration = cameraPositionParameter.transitionDuration;
        transitionDuration = 5;
        isInTransition = true;
        mainCamera.transform.DOMove(cameraPositions[(int)cameraLocation].cameraPosition.position, transitionDuration).onComplete = OnComplete ;
        mainCamera.DOOrthoSize(cameraPositionParameter.cameraSize, transitionDuration);
    }
}

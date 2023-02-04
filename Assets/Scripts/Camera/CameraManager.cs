using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
            cameraLocation++;
            MoveCamera();

        }
        else if (Input.mouseScrollDelta.y < 0 && cameraLocation != CameraLocation.Down)
        {
            cameraLocation--;
            MoveCamera();
        }
    }

    private void MoveCamera(bool overrideTransition = false, float overrideDuration = 0)
    {
        var cameraPositionParameter = cameraPositions[(int)cameraLocation];
        var transitionDuration = cameraPositionParameter.transitionDuration;
        if (overrideTransition)
            transitionDuration = overrideDuration;
        isInTransition = true;
        mainCamera.transform.DOMove(cameraPositions[(int)cameraLocation].cameraPosition.position, transitionDuration).onComplete = () => isInTransition = false;
        mainCamera.DOOrthoSize(cameraPositionParameter.cameraSize, transitionDuration);
    }

    public void ChangeCameraLocation(CameraLocation newLocation, bool overrideTransition = false, float overrideDuration = 0)
    {
        cameraLocation = newLocation;
        MoveCamera(overrideTransition, overrideDuration);
    }
}

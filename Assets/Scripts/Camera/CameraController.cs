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
public class CameraController : MonoBehaviour
{
    [SerializeField] private List<CameraPositionParameters> cameraPositions;
    private CameraLocation cameraLocation;
    private Camera camera;

    private bool isInTransition;

    void Start()
    {
        camera = GetComponent<Camera>();
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
        this.transform.DOMove(cameraPositions[(int)cameraLocation].cameraPosition.position, transitionDuration).onComplete = () => isInTransition = false;
        camera.DOOrthoSize(cameraPositionParameter.cameraSize, transitionDuration);
    }

    public void ChangeCameraLocation(CameraLocation newLocation, bool overrideTransition = false, float overrideDuration = 0)
    {
        cameraLocation = newLocation;
        MoveCamera(overrideTransition, overrideDuration);
    }
}

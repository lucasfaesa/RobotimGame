using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrbitalCamera : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private PinsManagerSO pinsManager;
    [Space] 
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform offsetReference;
    [Space]
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public float zoomDampening = 5.0f;

    [field:SerializeField] public bool CanOrbit { get; set; }
    
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float desiredOrthographicSize;
    
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
 
    private Vector3 FirstPosition;
    private Vector3 SecondPosition;
    private Vector3 delta;
    private Vector3 lastOffset;
    private Vector3 lastOffsettemp;

    private bool pinSelected;
    private Vector3 latestCameraPos;
    
    private PlayerInputActions InputActions { get; set; }
    private void Awake()
    {
        InputActions = new PlayerInputActions();
    }
    
    void Start() { Init(); }

    void OnEnable()
    {
        pinsManager.pinSelected += PinSelected;
        pinsManager.pinDeselected += PinDeselected;
            
        InputActions.Enable();
        Init();
    }

    private void OnDisable()
    {
        pinsManager.pinSelected -= PinSelected;
        pinsManager.pinDeselected -= PinDeselected;
            
        InputActions.Disable();
    }

    private void PinSelected(PinInfo n)
    {
        //offsetReference.transform.parent = null;
        if (pinSelected) return;
        
        latestCameraPos = cameraTransform.position;
        OffsetCameraToTheRight();
        pinSelected = true;
    }

    private void PinDeselected()
    {
        ResetCameraOffset();
    }

    private void OffsetCameraToTheRight()
    {
        var pos = offsetReference.transform.position;
        
        cameraTransform.DOMove(pos, 0.5f);
    }

    private void ResetCameraOffset()
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(cameraTransform.DOMove(latestCameraPos, 0.5f));
        sequence.OnComplete(() =>
        {
            pinSelected = false;
        });
    }
    
    private void Init()
    {
        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;
 
        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;
 
        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }
    
    void LateUpdate()
    {
        if (pinSelected) return;
        
        //ORBIT
        var leftClickValue = (int)InputActions.Player.LeftMouseClick.ReadValue<float>();
        bool clicked = leftClickValue == 0 ? false : true;
        
        if (clicked && CanOrbit)
        {
            xDeg += InputActions.Player.Look.ReadValue<Vector2>().x * xSpeed * 0.02f;
            yDeg -= InputActions.Player.Look.ReadValue<Vector2>().y * ySpeed * 0.02f;
             
            ////////OrbitAngle
            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            // set camera rotation
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;
            rotation = desiredRotation;
            transform.rotation = rotation;
            //Debug.Log("xDegree: "+ xDeg + " yDegree: " + yDeg + " // rotY: " + currentRotation.eulerAngles.y + " rotX: " + currentRotation.eulerAngles.x);
        }

        //Debug.Log("Desired Distance: " + desiredDistance + " current Distance: " + currentDistance);
         //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
         // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
         // calculate position based on the new currentDistance
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}

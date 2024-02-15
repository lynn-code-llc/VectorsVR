using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Vector : MonoBehaviour
{
    public float tipOffset;
    
    private Vector3 _startPoint;
    private GameObject _vectorEnd;

    private LineRenderer _xComponent;
    private LineRenderer _yComponent;
    private LineRenderer _zComponent;

    public GameObject startVector;
    
    public GameObject bodyPrefab; // Assign in inspector
    public GameObject tipPrefab; // Assign in inspector
    public GameObject pointPrefab;

    public Transform bodyInstance; 
    public Transform tipInstance;
    public Transform pointInstance;
    private bool _isActiveVector;

    public float _vectorMagnitude;
    [SerializeField] private Object vectorPrefab;
    public Vector3 tipEnd;
    [SerializeField] private bool _displayComponents;

    private void Awake()
    {
        
        // bodyPrefab = Resources.Load<GameObject>("Vector Body");
        // tipPrefab = Resources.Load<GameObject>("Vector Tip");
        // pointPrefab = Resources.Load<GameObject>("Point Prefab");
    }


    private void OnEnable()
    {
        tipOffset = 0.045f;
        
        bodyInstance = transform.Find("Body");
        tipInstance = transform.Find("Tip");
        pointInstance = transform.Find("Point");

        _xComponent = pointInstance.transform.Find("xComponent").GetComponent<LineRenderer>();
        _yComponent = pointInstance.transform.Find("yComponent").GetComponent<LineRenderer>();
        _zComponent = pointInstance.transform.Find("zComponent").GetComponent<LineRenderer>();
        
        _vectorEnd = pointInstance.gameObject;
    }

    private void Update()
    {
        UpdateVector();

        if (_displayComponents)
        {
            UpdateComponents();
        }

    }

    private void UpdateComponents()
    {
       _xComponent.SetPosition(0, Vector3.zero); 
       _xComponent.SetPosition(1, new Vector3(pointInstance.transform.position.x, 0, 0)); 
       
       _yComponent.SetPosition(0, new Vector3(pointInstance.transform.position.x, 0, 0)); 
       _yComponent.SetPosition(1, new Vector3(pointInstance.transform.position.x, pointInstance.transform.position.y, 0)); 
       
       _zComponent.SetPosition(0, new Vector3(pointInstance.transform.position.x, pointInstance.transform.position.y, 0)); 
       _zComponent.SetPosition(1, pointInstance.transform.position); 
    }

    public void InitializeVector(GameObject startPoint)
    {
        startVector = startPoint;
    }
    public void InitializeVector(bool isFirstVector)
    {
        if (isFirstVector)
        {
            startVector = new GameObject("Origin");
            startVector.transform.position = Vector3.zero;
            pointInstance.gameObject.SetActive(true);
            _vectorEnd = pointInstance.gameObject;
        }
    }

    public void SetVectorEnd(GameObject endPoint)
    {
        _vectorEnd = endPoint;
    }

    private void UpdateVector()
    {
        pointInstance.transform.position = _vectorEnd.transform.position;
        Debug.Log("Updating vector");
        // _vectorEnd = pointInstance.transform.position;
        // Vector3 direction = pointInstance.transform.position - startVector.transform.position;
        Vector3 direction = _vectorEnd.transform.position - startVector.transform.position;

        // Calculate the position for the tip that is sphereRadius units away from the sphere's surface
        // towards the vector's start point
        tipEnd = _vectorEnd.transform.position - direction.normalized * tipOffset;
        // Position tip
        tipInstance.transform.position = tipEnd;
        tipInstance.transform.up = direction;
        
        // Calculate the adjusted length for the vector body
        float adjustedLength = Vector3.Distance(startVector.transform.position, tipEnd);

        // Update the body's position to be centered between the start point and the new tip position
        bodyInstance.transform.position = startVector.transform.position + direction.normalized * (adjustedLength / 2.0f);
        bodyInstance.transform.up = direction;

        
        
        // float magnitude = direction.magnitude;
        // // Position and scale body
        // bodyInstance.transform.position = startVector.transform.position + (direction / 2.0f);
        // bodyInstance.transform.up = direction;
        bodyInstance.transform.localScale = new Vector3(bodyInstance.transform.localScale.x, adjustedLength / 2, bodyInstance.transform.localScale.z); // Adjust scale as needed
    }
}

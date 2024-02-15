using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CrossProductManager : MonoBehaviour
{
    public Vector VectorA { get; private set; }
    public Vector VectorB { get; private set; }
    
    public Vector CrossProduct { get; private set; }

    private GameObject _crossProductEnd;

    public Vector3 initialPointScale;
    
    [SerializeField] private List<Vector> _vectors = new List<Vector>();
    [SerializeField] private GameObject vectorPrefab;
    private bool isFirstVector;
    private CrossProductAreaMesh _crossProductMesh;

    // User should only be able to create one vector from an end point
    // Vector should be created when a user grabs the active vector

    private void OnEnable()
    {
        _crossProductMesh = GameObject.Find("Cross Product Mesh").GetComponent<CrossProductAreaMesh>();
        
        AddVectors();
    }

    private void Update()
    {
        if (VectorA != null && VectorB != null)
        {
            Vector3 crossProduct = Vector3.Cross(VectorA.tipEnd, VectorB.tipEnd);
            if (crossProduct.magnitude <= 0.1f)
            {
                _crossProductEnd.transform.position = Vector3.zero;
        }
            else
            {
                _crossProductEnd.transform.position = crossProduct;
            }
            
            
            Debug.Log(Vector3.Cross(VectorA.tipEnd, VectorB.tipEnd));
            Debug.Log(_crossProductEnd.transform.position.magnitude);
            
            _crossProductMesh.vectorA = VectorA.tipEnd;
            _crossProductMesh.vectorB = VectorB.tipEnd;
 
        }
    }

    public void AddVectors()
    {
        Debug.Log("Vector Added");
        if (_vectors.Count == 0)
        {
            VectorA = Instantiate(vectorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Vector>();
            
            VectorA.InitializeVector(isFirstVector:true);
            VectorA.pointInstance.transform.localScale = initialPointScale;
            
            _vectors.Add(VectorA);
            
            VectorA.bodyInstance.GetComponent<Renderer>().material.color = Color.blue;
            
            VectorA.pointInstance.GetComponent<XRGrabInteractable>().selectExited.AddListener(Ungrab);
        }
    }

    private void Ungrab(SelectExitEventArgs e)
    {
        if (_vectors.Count >= 2)
        {
            return;
        }
        
        VectorB = Instantiate(vectorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Vector>();
        VectorB.InitializeVector(isFirstVector:true);
        VectorB.pointInstance.transform.localScale = initialPointScale;
        _vectors.Add(VectorB);
        VectorB.bodyInstance.GetComponent<Renderer>().material.color = Color.blue;

        CrossProduct = Instantiate(vectorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Vector>();
        CrossProduct.InitializeVector(isFirstVector:true);
        _crossProductEnd = Instantiate(new GameObject("Cross Product End"));
        CrossProduct.SetVectorEnd(_crossProductEnd);
        
        CrossProduct.bodyInstance.GetComponent<Renderer>().material.color = Color.red;
        
        CrossProduct.pointInstance.gameObject.SetActive(false);
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VectorManager : MonoBehaviour
{
    public Vector PrevVector { get; private set; }
    public Vector FinalVector { get; private set; }

    public Vector3 initialPointScale;
    
    [SerializeField] private List<Vector> _vectors = new List<Vector>();
    [SerializeField] private GameObject vectorPrefab;
    private bool isFirstVector;

    // User should only be able to create one vector from an end point
    // Vector should be created when a user grabs the active vector

    private void OnEnable()
    {
        AddVector();
    }

    public void AddVector()
    {
        Debug.Log("Vector Added");
        if (_vectors.Count == 0)
        {
            FinalVector = Instantiate(vectorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Vector>();
            PrevVector = Instantiate(vectorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Vector>();
            
            FinalVector.InitializeVector(isFirstVector:true);
            FinalVector.pointInstance.transform.localScale = initialPointScale;
            
            PrevVector.InitializeVector(GameObject.Find("Origin"));
            PrevVector.SetVectorEnd(FinalVector.pointInstance.gameObject);
            
            FinalVector.gameObject.name = $"Vector {_vectors.Count + 1}";
            _vectors.Add(FinalVector);
            PrevVector.gameObject.name = $"Vector {_vectors.Count + 1}";
            _vectors.Add(PrevVector);
            // PrevVector = FinalVector;
            Debug.Log("Previous Vector = " + PrevVector.gameObject.name);
            
            FinalVector.pointInstance.GetComponent<XRGrabInteractable>().selectEntered.AddListener(AddVector);
            FinalVector.pointInstance.GetComponent<XRGrabInteractable>().selectExited.AddListener(UnGrab);
        }
    }

    // Call this method to add a new vector
    public void AddVector(SelectEnterEventArgs e)
    {
        if (!isFirstVector)
        {
            isFirstVector = true;
            return;
        }
        
        Debug.Log("Add Vector");

        Debug.Log($"Final Vector: {FinalVector.gameObject.name}");
        // PrevVector = _vectors[_vectors.Count - 1].gameObject;
        Vector newVector = Instantiate(vectorPrefab, FinalVector.pointInstance.transform.position, Quaternion.identity).GetComponent<Vector>();
        newVector.InitializeVector(startPoint:PrevVector.pointInstance.gameObject);
        newVector.SetVectorEnd(FinalVector.pointInstance.gameObject);
        
        newVector.gameObject.name = $"Vector {_vectors.Count + 1}";
        // PrevVector = newVector.gameObject;
        
        Debug.Log("Previous Vector = " + PrevVector.gameObject.name);
        
        _vectors.Add(newVector);
        PrevVector = _vectors.Last();
        // PrevVector.pointInstance.GetComponent<XRGrabInteractable>().enabled = true;

        // Configure the new vector's end position, appearance, etc.
    
        newVector.pointInstance.GetComponent<XRGrabInteractable>().selectEntered.AddListener(AddVector);
        newVector.pointInstance.GetComponent<XRGrabInteractable>().selectExited.AddListener(UnGrab);
    }

    public void UnGrab(SelectExitEventArgs e)
    {
        PrevVector = _vectors.Last();
        // PrevVector.pointInstance.GetComponent<XRGrabInteractable>().enabled = false;
        // Unsubscribe from Events
        PrevVector.GetComponent<Vector>().pointInstance.GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(AddVector);
        PrevVector.GetComponent<Vector>().pointInstance.GetComponent<XRGrabInteractable>().selectExited.RemoveListener(UnGrab);
        
        PrevVector.transform.Find("Point").gameObject.SetActive(true);
        PrevVector.GetComponent<Vector>().SetVectorEnd(PrevVector.pointInstance.gameObject);
    }
}



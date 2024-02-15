using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class HandleOrientation : MonoBehaviour
{
    void Update()
    {
        // Calculate the direction vector from the object to the origin
        Vector3 directionToOrigin = Vector3.zero - transform.position;
        
        // Normalize the direction vector
        directionToOrigin.Normalize();
        
        // Assume that the object's local Y-axis should point towards the origin.
        // We need to create a rotation that aligns the local Y-axis (0, 1, 0) with the directionToOrigin vector.
        // This can be achieved by using Quaternion.FromToRotation, passing the local Y-axis and the target direction.
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, directionToOrigin);
        
        // Apply the calculated rotation to the object
        transform.rotation = targetRotation;
    }
}



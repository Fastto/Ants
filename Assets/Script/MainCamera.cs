using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Vector3 DefaultPosition = new Vector3(0, 0, -10);

    private GameObject _trackingObject;
    private bool _isTracking = false;
    
    public void Track(GameObject obj)
    {
        _trackingObject = obj;
        _isTracking = true;
    }

    public void StopTracking()
    {
        _isTracking = false;
        transform.position = DefaultPosition;
    }

    private void Update()
    {
        if (_isTracking)
        {
            transform.position = new Vector3(_trackingObject.transform.position.x, _trackingObject.transform.position.y,
                DefaultPosition.z);
        }
    }
}

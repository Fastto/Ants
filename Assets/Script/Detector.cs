using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private Ant _ant;

    private void Awake()
    {
        _ant = GetComponentInParent<Ant>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("MarkerDetector") && (other.CompareTag("ToHome") || other.CompareTag("ToFood")))
        {
            _ant.OnMarkerFoundHandler(other);
        }
        else if (other.CompareTag("Home"))
        {
            if (CompareTag("ObjectDetector"))
            {
                _ant.OnHomeFoundHandler(other);
            }
            else if (CompareTag("CollisionDetector"))
            {
                _ant.OnHomeTouchedHandler(other);
            }
        }
        else if (other.CompareTag("Food"))
        {
            if (CompareTag("ObjectDetector"))
            {
                _ant.OnFoodFoundHandler(other);
            }
            else if (CompareTag("CollisionDetector"))
            {
                _ant.OnFoodTouchedHandler(other);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (CompareTag("MarkerDetector") && (other.CompareTag("ToHome") || other.CompareTag("ToFood")))
        {
            _ant.OnMarkerLostHandler(other);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && CompareTag("CollisionDetector"))
        {
            _ant.OnWallTouchHandler();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && CompareTag("CollisionDetector"))
        {
            _ant.OnWallTouchHandler();
        }
    }
}
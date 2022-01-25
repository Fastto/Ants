using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ant : MonoBehaviour
{
    [SerializeField] private GameObject ToHomeMarker;

    [SerializeField] private GameObject ToFoodMarker;

    [SerializeField] private int MarkersAmount = 200;

    [SerializeField] private float MarkersBurningRate = .9f;

    [SerializeField] private float TimeToMark = .2f;

    [SerializeField] private int Speed = 10;


    //For internal use
    private float _timeFromLastMark;
    private bool _isBusy;
    private int _markersAvailableAmount;
    private float _markersIntensivity;
    private float _markersIntensivityBurningRate;
    private Vector3 _targetPosition;
    private bool _goToTagretPosition;
    private bool _isOnWall;
    

    public Marker MostIntensiveToHomeMarker { get; set; }
    public Marker MostIntensiveToFoodMarker { get; set; }

    public List<Marker> ToFoodList { get; } = new List<Marker>();
    public List<Marker> ToHomeList { get; } = new List<Marker>();
    

    private void Start()
    {
        _timeFromLastMark += Random.Range(0, TimeToMark / 2);
        _isBusy = false;
        _markersAvailableAmount = MarkersAmount;
        _markersIntensivity = 1f;
        _markersIntensivityBurningRate = MarkersBurningRate;
        _goToTagretPosition = false;
        _isOnWall = false;

        Speed += Random.Range(-2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        _timeFromLastMark += Time.deltaTime;

        //Put Marker
        if (!_isOnWall && _timeFromLastMark > TimeToMark && _markersAvailableAmount > 0)
        {
            _timeFromLastMark = 0;
            Marker marker = Instantiate(_isBusy ? ToFoodMarker : ToHomeMarker, transform.position, Quaternion.identity)
                .GetComponentInChildren<Marker>();
            marker.Intensivity = _markersIntensivity;
            marker.Refresh();

            _markersAvailableAmount -= 1;
            _markersIntensivity *= _markersIntensivityBurningRate;
        }

        //Home Or Food have been seen, go there
        if (_goToTagretPosition)
        {
            transform.up = Vector3.Lerp(transform.up,
                (_targetPosition - transform.position).normalized, .5f);
        }
        //Is looking for food
        else if (!_isBusy)
        {
            if (MostIntensiveToFoodMarker != null)
            {
                Debug.DrawLine(transform.position, MostIntensiveToFoodMarker.transform.position);
                transform.up = Vector3.Lerp(transform.up,
                    (MostIntensiveToFoodMarker.transform.position - transform.position).normalized, Random.Range(.25f, .75f));
            }
        }
        //Is looking for home
        else
        {
            if (MostIntensiveToHomeMarker != null)
            {
                Debug.DrawLine(transform.position, MostIntensiveToHomeMarker.transform.position);
                transform.up = Vector3.Lerp(transform.up,
                    (MostIntensiveToHomeMarker.transform.position - transform.position).normalized, Random.Range(.25f, .75f));
            }
        }

        //Random direction change
        if (Random.Range(0, 100) > 90)
        {
            transform.Rotate(Vector3.forward, Random.Range(-30, 30));
        }

        //Return to sandbox area
        if (transform.position.magnitude > 100)
        {
            transform.position = transform.position * .98f;
            transform.up = -1 * transform.up;
            transform.Rotate(Vector3.forward, Random.Range(-15, 15));
        }
        
        //Do Step
        transform.position += transform.up * (Time.deltaTime * Speed);
    }

    private void FixedUpdate()
    {
        //Is looking for Home
        if (_isBusy)
        {
            if (ToHomeList.Count > 0)
            {
                MostIntensiveToHomeMarker = ToHomeList[0];
                foreach (Marker point in ToHomeList)
                {
                    if (point.Intensivity > MostIntensiveToHomeMarker.Intensivity)
                    {
                        MostIntensiveToHomeMarker = point;
                    }
                }
            }
            else
            {
                MostIntensiveToHomeMarker = null;
            }
        }
        //Is looking for Food
        else
        {
            if (ToFoodList.Count > 0)
            {
                MostIntensiveToFoodMarker = ToFoodList[0];
                foreach (Marker point in ToFoodList)
                {
                    if (point.Intensivity > MostIntensiveToFoodMarker.Intensivity)
                    {
                        MostIntensiveToFoodMarker = point;
                    }
                }
            }
            else
            {
                MostIntensiveToFoodMarker = null;
            }
        }
    }

    public void OnHomeFoundHandler(Collider2D other)
    {
        if (_isBusy)
        {
            _targetPosition = other.gameObject.transform.position;
            _goToTagretPosition = true;
        }
    }

    public void OnHomeTouchedHandler(Collider2D other)
    {
        if (_isBusy)
        {
            transform.up = -1 * transform.up;
            _isBusy = false;
            _markersAvailableAmount = MarkersAmount;
            _markersIntensivity = 1f;
            _markersIntensivityBurningRate = MarkersBurningRate;
            _goToTagretPosition = false;
            MostIntensiveToFoodMarker = null;
            ToFoodList.Clear();
        }
    }

    public void OnFoodFoundHandler(Collider2D other)
    {
        if (!_isBusy)
        {
            _targetPosition = other.gameObject.transform.position;
            _goToTagretPosition = true;
        }
    }

    public void OnFoodTouchedHandler(Collider2D other)
    {   if (!_isBusy)
        {
            transform.up = -1 * transform.up;
            _isBusy = true;
            _markersAvailableAmount = MarkersAmount;
            _markersIntensivity = 1f;
            _markersIntensivityBurningRate = MarkersBurningRate;
            _goToTagretPosition = false;
            MostIntensiveToHomeMarker = null;
            ToHomeList.Clear();
            
            other.GetComponentInParent<Food>().Bite();
        }
    }

    public void OnMarkerFoundHandler(Collider2D other)
    {
        if (_isBusy && other.CompareTag("ToHome"))
        {
            ToHomeList.Add(other.gameObject.GetComponentInChildren<Marker>());
        }
        else if (!_isBusy && other.CompareTag("ToFood"))
        {
            ToFoodList.Add(other.gameObject.GetComponentInChildren<Marker>());
        }
    }

    public void OnMarkerLostHandler(Collider2D other)
    {
        Marker marker = other.gameObject.GetComponentInChildren<Marker>();
        if (other.CompareTag("ToHome"))
        {
            ToHomeList.Remove(other.gameObject.GetComponentInChildren<Marker>());
        }
        else if (other.CompareTag("ToFood"))
        {
            ToFoodList.Remove(other.gameObject.GetComponentInChildren<Marker>());
        }
    }

    public void OnWallTouchHandler(Collider2D other)
    {
        _isOnWall = true;
        transform.up *= -1;
        transform.Rotate(Vector3.forward, Random.Range(-45, 45));
    }
    
    public void OnWallDetouchHandler(Collider2D other)
    {
        _isOnWall = false;
    }
}
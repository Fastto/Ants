using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarkerLayers
{
    private static int width = 1920;
    private static int height = 1080;

    private static Marker[,] _map;

    static MarkerLayers()
    {
        _map = new Marker[width, height];
    }

    static bool SetMarker(Marker marker)
    {
        Vector2Int coordinates = GetMarkerCoordinates(marker);
        
        if (_map[coordinates.x, coordinates.y] != null)
        {
            if (_map[coordinates.x, coordinates.y].Intensivity > marker.Intensivity)
            {
                marker.Remove();
                return false;
            }
            else
            {
                _map[coordinates.x, coordinates.y].Remove();
            }
        } 
        
        _map[coordinates.x, coordinates.y] = marker;
        return true;
    }

    static Vector2Int GetMarkerCoordinates(Marker marker)
    {
        return new Vector2Int((int) (marker.transform.position.x * 10), (int) (marker.transform.position.y * 10));
    }
}

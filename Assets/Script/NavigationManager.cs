using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    public Slider CameraSizeSlider;
    public Camera MainCamera;

    private void Start()
    {
        Scale = (int) MainCamera.orthographicSize;
    }

    public int Scale
    {
        get { return (int) CameraSizeSlider.value; }
        set { CameraSizeSlider.value = value; }
    }

    public void OnZoom()
    {
        MainCamera.orthographicSize = Scale;
    }
}